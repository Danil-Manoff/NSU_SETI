using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

class Searcher {
    private string multicastAddress;
    private int port;

    private IPEndPoint localEndPoint;
    private IPEndPoint remoteEndPoint; 
    private Dictionary<string, DateTime> aliveCopies = new Dictionary<string, DateTime>();
    private UdpClient udpClient = new UdpClient();

    private const int lifeTime = 3;

    public Searcher() {
        this.multicastAddress = "224.0.0.1";
        this.port = 12345;
        this.localEndPoint = new IPEndPoint(IPAddress.Any, this.port);
        this.remoteEndPoint = new IPEndPoint(IPAddress.Parse(this.multicastAddress), this.port);
        InitUdpClient(); 
    }
    public Searcher(string multicastAddress, int port) {
        this.multicastAddress = multicastAddress;
        this.port = port;
        this.localEndPoint = new IPEndPoint(IPAddress.Any, port);
        this.remoteEndPoint = new IPEndPoint(IPAddress.Parse(multicastAddress), port);
        InitUdpClient();
    }

    public string GetMulticastAddress() {return multicastAddress;}
    public int GetPort() {return port;}

    public IPEndPoint GetIPEndPoint() {return localEndPoint;}

    public void InitUdpClient() {
        this.udpClient.ExclusiveAddressUse = false;
        this.udpClient.Client.SetSocketOption(SocketOptionLevel.Socket,
                                            SocketOptionName.ReuseAddress,
                                            true);
        this.udpClient.Client.Bind(this.localEndPoint);
        this.udpClient.JoinMulticastGroup(IPAddress.Parse(this.multicastAddress));
    }

    public void RegisterCopy(string endPoint) {
        lock (aliveCopies) {
            if (!aliveCopies.ContainsKey(endPoint)) {
                aliveCopies.Add(endPoint, DateTime.Now);
            } else {
                aliveCopies[endPoint] = DateTime.Now;
            }
        }
    }
    
    public bool MyEquals(Dictionary<string, DateTime> first, Dictionary<string, DateTime> second) {
        foreach (var pairFirst in first) {
            if (!second.ContainsKey(pairFirst.Key)) {
                return false;
            }
        }
        return true;
    }

    public void PrintAliveCopies() {
        lock (aliveCopies) {
            DateTime now = DateTime.Now;
            Dictionary<string, DateTime> checkedAliveCopies = new Dictionary<string, DateTime>();
            foreach (var pair in aliveCopies) {
                if ((now - pair.Value).TotalSeconds < lifeTime) {
                    checkedAliveCopies.Add(pair.Key, pair.Value);
                }
            }
            aliveCopies = checkedAliveCopies;
            foreach (var pair in aliveCopies) {
                Console.WriteLine($"{pair.Key} still alive");
            }
            Console.WriteLine();
        }
    }

    public async void GetCopies() {
        while (true) {
            var result = await udpClient.ReceiveAsync();
            IPEndPoint receivedEndPoint = result.RemoteEndPoint;
            RegisterCopy(receivedEndPoint.ToString());
        }
    }

    public void Send() {
        byte[] message = Encoding.UTF8.GetBytes("Hello multicast!"); 
        udpClient.Send(message, message.Length, remoteEndPoint);
    }    

    public static HashSet<IPAddress> GetLocalIPAddresses()
    {
        HashSet<IPAddress> ipAddresses = new HashSet<IPAddress>();

        foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
        {
            // Пропускаем интерфейсы, которые не работают
            if (ni.OperationalStatus != OperationalStatus.Up)
                continue;

            var ipProps = ni.GetIPProperties();
            foreach (UnicastIPAddressInformation addr in ipProps.UnicastAddresses)
            {
                // Добавляем только IPv4 адреса
                if (addr.Address.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipAddresses.Add(addr.Address);
                }
            }
        }

        return ipAddresses;
    }
}
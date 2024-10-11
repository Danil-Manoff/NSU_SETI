using System.Net;
using System.Net.Sockets;
using System.Text;

public class UdpSender {
    public async void StartAsyncUdpSender(string iPAddress) {
        Socket udpSoket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        string message = "Hello, Man!\n";
        byte[] data = Encoding.UTF8.GetBytes(message);
        IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Parse(iPAddress), 12345);
        int bytes = await udpSoket.SendToAsync(data, remoteIPEndPoint);
        Console.WriteLine($"Отправлено {bytes} байт");

    }

    public void StartSyncUdpSender(string iPAddress) {
        
        Socket udpSoket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        string message = "Hello, Man!\n";
        byte[] data = Encoding.UTF8.GetBytes(message);
        IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Parse(iPAddress), 12345);
        int bytes = udpSoket.SendTo(data, remoteIPEndPoint);
        Console.WriteLine($"Отправлено {bytes} байт");

    }
}
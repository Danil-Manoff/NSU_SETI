using System.Net;
using System.Net.Sockets;
using System.Text;

public class UdpReciever {
    public async void StartAsyncUdpReciever(string iPAddress) {
        Socket udpSoket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        /*
        AddressFamily:
            InterNetwork: Address for IPv4
            InterNetworkv6: Address for IPv6
            ...
        
        SocketType:
            Dgram   - default for UDP
            Stream  - default for TCP
            ...

        ProtocolType:
            TCP
            UDP
            ...
        */
        
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(iPAddress), 12345);
        udpSoket.Bind(endPoint);
        Console.WriteLine("UDP-сервер запущен!\n");

        byte[] data = new byte[256];
        EndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);

        SocketReceiveFromResult result = await udpSoket.ReceiveFromAsync(data, remoteIPEndPoint);
        String message = Encoding.UTF8.GetString(data, 0, result.ReceivedBytes);

        Console.WriteLine($"Получено {result.ReceivedBytes} байт");
        Console.WriteLine($"Удаленный адрес: {result.RemoteEndPoint}");
        Console.WriteLine(message);
    }

    public void StartSyncUdpReciever(string iPAddress) {
        Socket udpSoket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(iPAddress), 12345);
        udpSoket.Bind(endPoint);
        Console.WriteLine("UDP-сервер запущен!\n");

        byte[] data = new byte[256];
        EndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);

        int receivedBytes = udpSoket.ReceiveFrom(data, ref remoteIPEndPoint);
        String message = Encoding.UTF8.GetString(data, 0, receivedBytes);

        Console.WriteLine($"Получено {receivedBytes} байт");
        Console.WriteLine($"Удаленный адрес: {remoteIPEndPoint}");
        Console.WriteLine(message);
    }
}

/*
Socket udpSoket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
    Arguments:
        AddressFamily:
            InterNetwork: Address for IPv4
            InterNetworkv6: Address for IPv6
            ...
        
        SocketType:
            Dgram   - default for UDP
            Stream  - default for TCP
            ...

        ProtocolType:
            TCP
            UDP
            ... 
*/

using System.Runtime.CompilerServices;

class Program {
    static void Main(string[] args) {
        // string firstHostIPAddress = "127.0.0.1";
        // string secondHostIPAddress = "127.0.0.2";

        string myAddr = args[0];
        string remoteAddr = args[1];
        string flag = args[2];

        if (flag == "0") {
            new UdpReciever().StartSyncUdpReciever(myAddr);
        } else {
            new UdpSender().StartSyncUdpSender(remoteAddr);
        }
    }
}
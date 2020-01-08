using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;

namespace KP_lb17_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var r = new Random();
            int[] array = Enumerable.Range(0, 5).Select(x => r.Next(-10,10)).ToArray();
            Console.WriteLine("Data:" + string.Join(",", array) + "!");
            IPAddress ip = IPAddress.Loopback;
            int port = 8080;
            IPEndPoint endPoint = new IPEndPoint(ip, port);
            try
            {
                using (Socket sk = new Socket(SocketType.Stream, ProtocolType.Tcp))
                {
                    sk.Connect(endPoint);
                    var fr = new BinaryFormatter();
                    using (NetworkStream st = new NetworkStream(sk))
                    {
                        fr.Serialize(st, array);
                        int[] proccesed = fr.Deserialize(st) as int[];
                        Console.WriteLine("Proccesed:" + string.Join(",", proccesed) + "!");
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadKey();
        }
    }
}

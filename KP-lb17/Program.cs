using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace KP_lb17
{
    class Program
    {
        static int[] Procces(int[] ar)
        {
            int[] tmp = new int[ar.Length];
            int result = 0;
            for (int i = 0; i < ar.Length; i++)
            {
                int x = ar[i];
                result = Math.Abs(x * x - x * x * x) - ((7 * x) / (x * x * x - 15 * x));
                tmp[i] = result;
            }
            return tmp;
        }
        static async void ProccesRqAsync(IPEndPoint ep)
        {
            try
            {
                using (Socket sc = new Socket(SocketType.Stream, ProtocolType.Tcp))
                {
                    sc.Bind(ep);
                    sc.Listen(10);
                    Console.WriteLine("Waiting for connection!");
                    while (true)
                        using (Socket handler = await sc.AcceptAsync())
                        {
                            Console.WriteLine($"Connected:{(handler.LocalEndPoint as IPEndPoint).ToString()}");
                            Console.WriteLine("Get data:");
                            using (NetworkStream st = new NetworkStream(handler))
                            {
                                var fr = new BinaryFormatter();
                                var dataSet = fr.Deserialize(st) as int[];
                                Console.WriteLine("Data:" + string.Join(",", dataSet) + "!");
                                var proccesed = Procces(dataSet);
                                Console.WriteLine("Proccesed:" + string.Join(",", proccesed) + "!");
                                fr.Serialize(st, proccesed);
                                Console.WriteLine("Done!");
                                Console.WriteLine("Waiting for connection!");
                            }
                        }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        static void Main(string[] args)
        {
            IPAddress ip = IPAddress.Loopback;
            int port = 8080;
            IPEndPoint endPoint = new IPEndPoint(ip, port);
            ProccesRqAsync(endPoint);
            Console.ReadKey();
        }
    }
}

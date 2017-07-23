using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            binding.CloseTimeout = new TimeSpan(0, 10, 0);
            binding.OpenTimeout = new TimeSpan(0, 10, 0);
            binding.ReceiveTimeout = new TimeSpan(0, 10, 0);
            binding.SendTimeout = new TimeSpan(0, 10, 0);

            string address = "net.tcp://localhost:9999/WcfService";

            using (ClientProxy proxy = new ClientProxy(binding, new EndpointAddress(new Uri(address))))
            {
               

                Console.WriteLine("\nRequest for editing student remaining courses.");
                string ret = proxy.AccessDenied();
                Console.WriteLine(ret);

                Console.WriteLine("Request for student remaining courses.");
                ret = proxy.AccessPermit();
                Console.WriteLine(ret);
            }

            Console.ReadKey();
        }
    }
}

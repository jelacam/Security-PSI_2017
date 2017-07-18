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
            string address = "net.tcp://localhost:9999/WcfService";

            using (ClientProxy proxy = new ClientProxy(binding, new EndpointAddress(new Uri(address))))
            {
                string ret = proxy.Test();
                Console.WriteLine(ret);
            }

            Console.ReadKey();
        }
    }
}

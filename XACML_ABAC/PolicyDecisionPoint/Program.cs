using Contracts;
using Contracts.Contracts;
using PolicyDecisionPoint.XACML_CombAlg;
using PolicyDecisionPoint.XAML_Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PolicyDecisionPoint
{
    class Program
    {
        public static PolicyType policy = new PolicyType();

        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            binding.CloseTimeout = new TimeSpan(0, 10, 0);
            binding.OpenTimeout = new TimeSpan(0, 10, 0);
            binding.ReceiveTimeout = new TimeSpan(0, 10, 0);
            binding.SendTimeout = new TimeSpan(0, 10, 0);
            binding.MaxBufferPoolSize = 20000000;
            binding.MaxBufferSize = 20000000;
            binding.MaxConnections = 20000000;
            binding.MaxReceivedMessageSize = 20000000;
            string address = "net.tcp://localhost:8000/PdpService";

            ServiceHost host = new ServiceHost(typeof(ContextHandler));
            host.AddServiceEndpoint(typeof(IContextHandler), binding, address);

            host.Description.Behaviors.Remove(typeof(ServiceDebugBehavior));
            host.Description.Behaviors.Add(new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });

            

            try
            {
                host.Open();
                Console.WriteLine("PdpService is opened. Press <enter> to finish ... ");


                string PAPaddress = "net.tcp://localhost:6000/PapService";

                using (PapProxy proxy = new PapProxy(binding, new EndpointAddress(new Uri(PAPaddress))))
                {
                    policy = proxy.LoadPolicy();
                    Console.WriteLine("Policy loaded...");
                }

                Console.ReadLine();
                host.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while trying to stablish connection with service. {0}", e.Message);
            }
        }
    }
}

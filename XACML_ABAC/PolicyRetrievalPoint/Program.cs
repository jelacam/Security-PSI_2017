using Contracts.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace PolicyAdministrationPoint
{
    public class Program
    {
        private static void Main(string[] args)
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

            string address = "net.tcp://localhost:6000/PapService";

            ServiceHost host = new ServiceHost(typeof(PapService));
            host.AddServiceEndpoint(typeof(IPrpContract), binding, address);

            host.Description.Behaviors.Remove(typeof(ServiceDebugBehavior));
            host.Description.Behaviors.Add(new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });

            try
            {
                host.Open();
                Console.WriteLine("WcfService is opened. Press <enter> to finish ... ");
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
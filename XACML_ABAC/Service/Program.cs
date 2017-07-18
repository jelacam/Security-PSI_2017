using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Contracts;
using System.ServiceModel.Description;
using PolicyEnforcementPoint;

namespace Service
{
    public class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:9999/WCFService";

            ServiceHost host = new ServiceHost(typeof(WcfService));
            host.AddServiceEndpoint(typeof(IContract), binding, address);

            host.Description.Behaviors.Remove(typeof(ServiceDebugBehavior));
            host.Description.Behaviors.Add(new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });

            // podsavanje koriscenja custom managera
            host.Authorization.ServiceAuthorizationManager = new PepAuthorizationManager();

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

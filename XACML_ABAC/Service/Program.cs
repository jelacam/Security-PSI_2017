using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Contracts;
using System.ServiceModel.Description;
using PolicyEnforcementPoint;
using PolicyEnforcementPoint.CustomPrincipal;
using System.IdentityModel.Policy;

namespace Service
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var time = DateTime.Now;

            NetTcpBinding binding = new NetTcpBinding();
            binding.CloseTimeout = new TimeSpan(0, 10, 0);
            binding.OpenTimeout = new TimeSpan(0, 10, 0);
            binding.ReceiveTimeout = new TimeSpan(0, 10, 0);
            binding.SendTimeout = new TimeSpan(0, 10, 0);

            string address = "net.tcp://localhost:9999/WCFService";

            ServiceHost host = new ServiceHost(typeof(WcfService));
            host.AddServiceEndpoint(typeof(IContract), binding, address);

            host.Description.Behaviors.Remove(typeof(ServiceDebugBehavior));
            host.Description.Behaviors.Add(new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });

            // podsavanje koriscenja custom managera
            host.Authorization.ServiceAuthorizationManager = new PepAuthorizationManager();

            // podesavanje koriscenja custom polisa
            List<IAuthorizationPolicy> policies = new List<IAuthorizationPolicy>();
            policies.Add(new CustomAuthorization());
            host.Authorization.ExternalAuthorizationPolicies = policies.AsReadOnly();

            //host.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.Custom;
            host.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.UseWindowsGroups;

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
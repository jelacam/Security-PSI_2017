using Contracts.Contracts;
using PolicyEnforcementPoint.CustomPrincipal;
using System;
using System.Collections.Generic;
using System.IdentityModel.Policy;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService
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

            string address = "net.tcp://localhost:5000/AuthenticationService";

            ServiceHost host = new ServiceHost(typeof(AuthService));
            host.AddServiceEndpoint(typeof(IAuthentication), binding, address);

            host.AddServiceEndpoint(typeof(ISubjectInformation), binding, address);

            host.Description.Behaviors.Remove(typeof(ServiceDebugBehavior));
            host.Description.Behaviors.Add(new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });

            // podesavanje koriscenja custom polisa
            List<IAuthorizationPolicy> policies = new List<IAuthorizationPolicy>();
            policies.Add(new AuthzPolicy());
            host.Authorization.ExternalAuthorizationPolicies = policies.AsReadOnly();

            //host.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.Custom;
            host.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.UseWindowsGroups;

            try
            {
                host.Open();
                Console.WriteLine("Authentication service is opened. Press <enter> to finish ... ");
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
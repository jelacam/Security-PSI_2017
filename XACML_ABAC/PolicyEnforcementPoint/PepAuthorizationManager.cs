using Contracts;
using System;
using System.Collections.Generic;
using System.IdentityModel.Policy;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IdentityModel.Claims;
using System.Threading;
using System.Security.Principal;


namespace PolicyEnforcementPoint
{

    public class PepAuthorizationManager : ServiceAuthorizationManager
    {
        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            // System.IdentityModel.Policy.AuthorizationContext authContext = operationContext.ServiceSecurityContext.AuthorizationContext;
            //ClaimsPrincipal currentClaimsPrincipal = Thread.CurrentPrincipal as ClaimsPrincipal;

            //System.Security.Claims.AuthorizationContext authzContext = OperationContext.Current.RequestContext.
            //IIdentity indentity = operationContext.ServiceSecurityContext.AuthorizationContext.Properties["Identities"] as IIdentity;



            //var cc = operationContext.ServiceSecurityContext.AuthorizationContext.ClaimSets;

            string Subject = operationContext.ServiceSecurityContext.PrimaryIdentity.Name.Split('\\')[1];


            string[] Attributes = operationContext.RequestContext.RequestMessage.Headers.Action.Split('_');



            // service binding i adress
            NetTcpBinding binding = new NetTcpBinding();
            binding.CloseTimeout = new TimeSpan(0, 10, 0);
            binding.OpenTimeout = new TimeSpan(0, 10, 0);
            binding.ReceiveTimeout = new TimeSpan(0, 10, 0);
            binding.SendTimeout = new TimeSpan(0, 10, 0);
            string address = "net.tcp://localhost:8000/PdpService";

            DecisionType decision = DecisionType.Indeterminate;

            Dictionary<string, List<DomainAttribute>> DomainAttributes = new Dictionary<string, List<DomainAttribute>>();
            DomainAttributes["urn:oasis:names:tc:xacml:3.0:attribute-category:action"] = new List<DomainAttribute>()
            {
                new DomainAttribute() { AttributeId = "urn:oasis:names:tc:xacml:1.0:action:action-id", DataType=  "http://www.w3.org/2001/XMLSchema#string", Value = Attributes[0].ToLower() }
            };

            DomainAttributes["urn:oasis:names:tc:xacml:3.0:attribute-category:resource"] = new List<DomainAttribute>()
            {
                new DomainAttribute() { AttributeId = "urn:oasis:names:tc:xacml:1.0:resource:resource-id", DataType =  "http://www.w3.org/2001/XMLSchema#string", Value = Attributes[1].ToLower() }
            };

            DomainAttributes["urn:oasis:names:tc:xacml:3.0:subject-category:access-subject"] = new List<DomainAttribute>()
            {
                new DomainAttribute() { AttributeId = "urn:oasis:names:tc:xacml:1.0:subject:subject-id", DataType =  "http://www.w3.org/2001/XMLSchema#string", Value = Subject },
                new DomainAttribute() { AttributeId = "urn:oasis:names:tc:xacml:1.0:subject:subject-location", DataType =  "http://www.w3.org/2001/XMLSchema#string", Value = "Novi Sad"}
            };




            using (ContextProxy proxy = new ContextProxy(binding, new EndpointAddress(new Uri(address))))
            {
               decision = proxy.CheckAccess(DomainAttributes);
            }

            //Console.WriteLine("\n" + decision.ToString());


            if (decision == DecisionType.Permit)
            {
                return true;
            }
            else
            {
                return false;
            }

            




            //Console.ReadKey();
        }
    }
}

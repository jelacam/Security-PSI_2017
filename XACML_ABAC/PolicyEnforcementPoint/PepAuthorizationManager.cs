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

            //WindowsIdentity identity = operationContext.ClaimsPrincipal.Identity as WindowsIdentity;
            //foreach(IdentityReference group in identity.Groups)
            //{
            //    SecurityIdentifier sid = group.Translate(typeof(SecurityIdentifier)) as SecurityIdentifier;
            //    var name = sid.Translate(typeof(NTAccount));
            //}

            // service binding i adress
            NetTcpBinding binding = new NetTcpBinding();
            binding.CloseTimeout = new TimeSpan(0, 10, 0);
            binding.OpenTimeout = new TimeSpan(0, 10, 0);
            binding.ReceiveTimeout = new TimeSpan(0, 10, 0);
            binding.SendTimeout = new TimeSpan(0, 10, 0);
            string address = "net.tcp://localhost:8000/PdpService";

            DecisionType decision = DecisionType.Indeterminate;

            Dictionary<string, List<DomainAttribute>> DomainAttributes = new Dictionary<string, List<DomainAttribute>>();
            DomainAttributes[Contracts.XacmlAction.CATEGORY] = new List<DomainAttribute>()
            {
                new DomainAttribute() { AttributeId = Contracts.XacmlAction.ID, DataType = XacmlDataTypes.STRING, Value = Attributes[0].ToLower() }
            };

            DomainAttributes[XacmlResource.CATEGORY] = new List<DomainAttribute>()
            {
                new DomainAttribute() { AttributeId = XacmlResource.ID, DataType = XacmlDataTypes.STRING, Value = Attributes[1].ToLower() }
            };

            DomainAttributes[XacmlSubject.CATEGORY] = new List<DomainAttribute>()
            {
                new DomainAttribute() { AttributeId = XacmlSubject.ID, DataType = XacmlDataTypes.STRING, Value = Subject },
                new DomainAttribute() { AttributeId = XacmlSubject.LOCATION, DataType = XacmlDataTypes.STRING, Value = "Novi Sad"}
            };




            using (ContextProxy proxy = new ContextProxy(binding, new EndpointAddress(new Uri(address))))
            {
               decision = proxy.CheckAccess(DomainAttributes);
            }

           
            Console.WriteLine("PEP response: {0}", decision.ToString());

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

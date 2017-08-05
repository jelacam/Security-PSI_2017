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
using System.ServiceModel.Channels;
using System.Net;
using System.Xml;

namespace PolicyEnforcementPoint
{
    public class PepAuthorizationManager : ServiceAuthorizationManager
    {
        public object locker = new object();

        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            IPrincipal principal = operationContext.ServiceSecurityContext.AuthorizationContext.Properties["Principal"] as IPrincipal;

            CustomPrincipal.CustomPrincipal customPrincipal = principal as CustomPrincipal.CustomPrincipal;

            //lock (locker)
            //{
            //    IContractCallback callback = null;
            //    callback = OperationContext.Current.GetCallbackChannel<IContractCallback>();
            //    string test = callback.RequestClientIpAddress();
            //}
            var context = OperationContext.Current;
            var mp = context.IncomingMessageProperties;
            var propName = RemoteEndpointMessageProperty.Name;
            var prop = (RemoteEndpointMessageProperty)mp[propName];
            string remoteIP = prop.Address;

            using (var objClient = new System.Net.WebClient())
            {
                var strFile = objClient.DownloadString("http://freegeoip.net/xml/93.86.238.5");
            }

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

            // dodavanje atributa koji definise akciju
            DomainAttributes["action"] = new List<DomainAttribute>()
            {
                new DomainAttribute() { AttributeId = "action-id", DataType = "string", Value = Attributes[0].ToLower() }
            };

            DomainAttributes["resource"] = new List<DomainAttribute>()
            {
                new DomainAttribute() { AttributeId = "resource-id", DataType = "string", Value = Attributes[1].ToLower() }
            };

            // setovanje lokacije u PEP
            //DomainAttributes[XacmlSubject.CATEGORY] = new List<DomainAttribute>()
            //{
            //    //new DomainAttribute() { AttributeId = XacmlSubject.ID, DataType = XacmlDataTypes.STRING, Value = Subject },
            //    new DomainAttribute() { AttributeId = XacmlSubject.LOCATION, DataType = XacmlDataTypes.STRING, Value = "Novi Sad" }
            //};

            DomainAttributes["subject"] = new List<DomainAttribute>();
            foreach (string group in customPrincipal.Groups)
            {
                DomainAttributes["subject"].Add(new DomainAttribute() { AttributeId = "subject-role", DataType = "string", Value = group });
            }

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
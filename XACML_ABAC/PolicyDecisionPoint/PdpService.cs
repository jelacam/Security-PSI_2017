using Common;
using PolicyDecisionPoint.XACML_CombAlg;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PolicyDecisionPoint
{
    public static class PdpService
    {
        public static Dictionary<string, PolicyCombiningAlg> PolicyCombAlg = new Dictionary<string, PolicyCombiningAlg>(3);
        public static PolicySetType policySet = new PolicySetType();

        public static ResponseType Evaluate(RequestType request)
        {
            PolicyCombAlg[XacmlPolicyCombAlg.FIRST_APPLICABLE] = new FirstApplicablePolicy();

            /// deserijalizacija xml dokumenta koji specificira autorizacionu politiku
            //XmlSerializer serializer = new XmlSerializer(typeof(PolicyType));
            //StreamReader reader = new StreamReader("rule1.main.xml");
            ////StreamReader reader = new StreamReader("TimeRange.checkTimeInRange.xml");
            //var value = serializer.Deserialize(reader);

            ///// kreiranje objekta koji predstavlja definisanu politiku
            //PolicyType policy = new PolicyType();
            //policy = value as PolicyType;

            NetTcpBinding binding = new NetTcpBinding();
            binding.CloseTimeout = new TimeSpan(0, 10, 0);
            binding.OpenTimeout = new TimeSpan(0, 10, 0);
            binding.ReceiveTimeout = new TimeSpan(0, 10, 0);
            binding.SendTimeout = new TimeSpan(0, 10, 0);
            string address = "net.tcp://localhost:6000/PapService";

            if (policySet.Items == null)
            {
                using (PapProxy proxy = new PapProxy(binding, new EndpointAddress(new Uri(address))))
                {
                    policySet = proxy.Load();
                    Console.WriteLine("-------------------------------------");
                    Console.WriteLine("\nPolicy loaded...");
                }
            }
            List<PolicyType> policiesL = new List<PolicyType>();
            foreach (PolicyType policy in policySet.Items)
            {
                policiesL.Add(policy);
            }

            PolicyType[] policies = policiesL.ToArray();

            DecisionType decision = PolicyCombAlg[policySet.PolicyCombiningAlgId].Evaluate(policies, request);

            Console.WriteLine("\nAccess decision: {0}", decision.ToString());

            ResponseType XacmlResponse = new ResponseType();

            ResultType result = new ResultType();
            result.Decision = decision;
            XacmlResponse.Result = new ResultType[1];
            XacmlResponse.Result[0] = result;

            return XacmlResponse;
        }
    }
}
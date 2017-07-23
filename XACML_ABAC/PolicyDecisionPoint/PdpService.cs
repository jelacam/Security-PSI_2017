using Contracts;
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
        public static ResponseType Evaluate(RequestType request)
        {
            /// deserijalizacija xml dokumenta koji specificira autorizacionu politiku
            //XmlSerializer serializer = new XmlSerializer(typeof(PolicyType));
            //StreamReader reader = new StreamReader("rule1.main.xml");
            ////StreamReader reader = new StreamReader("TimeRange.checkTimeInRange.xml");
            //var value = serializer.Deserialize(reader);

            ///// kreiranje objekta koji predstavlja definisanu politiku 
            //PolicyType policy = new PolicyType();
            //policy = value as PolicyType;





            List<RuleType> rulesL = new List<RuleType>();
            foreach (RuleType rule in Program.policy.Items)
            {
                rulesL.Add(rule);
            }

            RuleType[] rules = rulesL.ToArray();


            FirstApplicable firstAppl = new FirstApplicable();
            DecisionType decision = firstAppl.firstApplicableEffectRuleCombiningAlgorithm(rules, request);

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

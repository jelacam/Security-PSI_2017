using PolicyDecisionPoint.XACML_CombAlg;
using PolicyDecisionPoint.XAML_Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PolicyDecisionPoint
{
    class Program
    {
        static void Main(string[] args)
        {
            /// deserijalizacija xml dokumenta koji specificira autorizacionu politiku
            XmlSerializer serializer = new XmlSerializer(typeof(PolicyType));
            StreamReader reader = new StreamReader("rule1.main.xml");
            var value = serializer.Deserialize(reader);

            /// kreiranje objekta koji predstavlja definisanu politiku 
            PolicyType policy = new PolicyType();
            policy = value as PolicyType;


            XmlSerializer ser1 = new XmlSerializer(typeof(RequestType));
            StreamReader re1 = new StreamReader("request.example.xml");
            var val = ser1.Deserialize(re1);

            RequestType request = new RequestType();
            request = val as RequestType;

            List<RuleType> rulesL = new List<RuleType>();
            foreach(RuleType rule in policy.Items)
            {
                rulesL.Add(rule);
            }

            RuleType[] rules = rulesL.ToArray();

            FirstApplicable firstAppl = new FirstApplicable();
            DecisionType decision = firstAppl.firstApplicableEffectRuleCombiningAlgorithm(rules, request);

            Console.WriteLine(decision.ToString());
            Console.ReadKey();
        }
    }
}

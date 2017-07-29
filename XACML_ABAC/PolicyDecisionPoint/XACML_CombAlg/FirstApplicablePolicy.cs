using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolicyDecisionPoint.XACML_CombAlg
{
    public class FirstApplicablePolicy : PolicyCombiningAlg
    {
        public Dictionary<string, RuleCombiningAlg> RuleCombiningAlg = new Dictionary<string, RuleCombiningAlg>(3);

        public FirstApplicablePolicy()
        {
            RuleCombiningAlg[XacmlRuleCombAlg.FIRST_APPLICABLE] = new FirstApplicableRule();
        }

        public override DecisionType Evaluate(PolicyType[] policies, RequestType request)
        {
            List<RuleType> rulesL = new List<RuleType>(3);

            foreach (PolicyType policy in policies)
            {
                foreach (RuleType rule in policy.Items)
                {
                    rulesL.Add(rule);
                }

                RuleType[] rules = rulesL.ToArray();

                rulesL.Clear();

                DecisionType decision = RuleCombiningAlg[policy.RuleCombiningAlgId].Evaluate(rules, request);

                Console.WriteLine("\n==>Policy decision: {0}", decision.ToString());
                Console.WriteLine("====================================");

                if (decision == DecisionType.Deny)
                {
                    return DecisionType.Deny;
                }
                else if (decision == DecisionType.Permit)
                {
                    return DecisionType.Permit;
                }
                else if (decision == DecisionType.NotApplicable)
                {
                    continue;
                }
                else if (decision == DecisionType.Indeterminate)
                {
                    return DecisionType.Indeterminate;
                }
            }

            return DecisionType.NotApplicable;
        }
    }
}
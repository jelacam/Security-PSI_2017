using PolicyDecisionPoint.XAML_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolicyDecisionPoint.XACML_CombAlg
{
    public class FirstApplicable
    {
        private string identifier = "urn:oasis:names:tc:xacml:1.0:rule-combining-algorithm:first-applicable";

        public string Identifier { get => identifier; set => identifier = value; }


        /// <summary>
        ///     Metoda koja implementira logiku donosenja odluke algoritmom FirstApplicable kod pravila  
        /// </summary>
        /// <param name="rules"></param>
        /// <returns></returns>
        public DecisionType firstApplicableEffectRuleCombiningAlgorithm(RuleType[] rules, RequestType request)
        {
            DecisionType decision = DecisionType.NotApplicable;

            foreach(RuleType rule in rules)
            {
                // metoda koja evaluira pravilo 
                decision = PolicyEvaluateManager.FirstApplRuleEvaluate(request, rule);

                if (decision.Equals(DecisionType.Deny))
                {
                    return DecisionType.Deny;
                }

                if(decision.Equals(DecisionType.Permit))
                {
                    return DecisionType.Permit;
                }

                if (decision.Equals(DecisionType.Indeterminate))
                {
                    return DecisionType.Indeterminate;
                }
               
            }

            return DecisionType.NotApplicable;
        }
    }
}

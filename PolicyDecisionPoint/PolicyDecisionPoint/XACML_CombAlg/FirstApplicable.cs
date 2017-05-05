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
        Decision firstApplicableEffectRuleCombiningAlgorithm(RuleType[] rules)
        {
            Decision decision = Decision.NotApplicable;

            foreach(RuleType rule in rules)
            {
                //decision = evaluate(rule); // metoda koja evaluira pravilo

                if (decision.Equals(Decision.Deny))
                {
                    return Decision.Deny;
                }

                if(decision.Equals(Decision.Permit))
                {
                    return Decision.Permit;
                }

                if (decision.Equals(Decision.Indeterminate))
                {
                    return Decision.Indeterminate;
                }
               
            }

            return Decision.NotApplicable;
        }
    }
}

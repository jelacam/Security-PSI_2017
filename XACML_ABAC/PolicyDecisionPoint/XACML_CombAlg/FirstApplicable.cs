using PolicyDecisionPoint.XAML_Common;
using System;


namespace PolicyDecisionPoint.XACML_CombAlg
{
    public class FirstApplicable
    {
        private string identifier = "urn:oasis:names:tc:xacml:1.0:rule-combining-algorithm:first-applicable";

        public string Identifier { get => identifier; set => identifier = value; }

        /// <summary>
        ///     Metoda koja implementira logiku donosenja odluke algoritmom FirstApplicable
        /// </summary>
        /// <param name="rules"></param>
        /// <returns></returns>
        public DecisionType firstApplicableEffectRuleCombiningAlgorithm(RuleType[] rules, RequestType request)
        {
            DecisionType decision = DecisionType.NotApplicable;

            foreach (RuleType rule in rules)
            {
                Console.WriteLine("\n-->Rule Id: {0}<--", rule.RuleId.ToString());
                // metoda koja evaluira pravilo
                try
                {
                    decision = PolicyEvaluateManager.RuleEvaluate(request, rule);
                    Console.WriteLine("PDP decision: {0}", decision.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: {0}", e.Message);
                    return DecisionType.Indeterminate;
                }

                if (decision.Equals(DecisionType.Deny))
                {
                    return DecisionType.Deny;
                }

                if (decision.Equals(DecisionType.Permit))
                {
                    return DecisionType.Permit;
                }

                if (decision.Equals(DecisionType.NotApplicable))
                {
                    continue;
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
using Contracts;
using System;
using System.ServiceModel;

namespace PolicyDecisionPoint.XAML_Common
{
    public static class PolicyEvaluateManager
    {
        /// <summary>
        ///     Metoda vrsi evaluaciju po principu definisanim XACML standardom: Sekcija C.8 First-applicable
        /// </summary>
        /// <param name="request"></param>
        /// <param name="rule"></param>
        /// <returns></returns>
        public static DecisionType RuleEvaluate(RequestType request, RuleType rule)
        {
            DecisionType Effect = DecisionType.Permit;

            if (rule.Effect.Equals(EffectType.Permit))
            {
                Effect = DecisionType.Permit;
            }
            else if (rule.Effect.Equals(EffectType.Deny))
            {
                Effect = DecisionType.Deny;
            }

            string AllOfValue = string.Empty;
            string AnyOfValue = string.Empty;

            TargetType Target = rule.Target;

            ConditionType Condition = rule.Condition;

            try
            {
                TargetResult TargetValue = TargetEvaluate.CheckTarget(Target, request);

                ConditionResult ConditionValue;

                if (TargetValue == TargetResult.NoMatch)
                {
                    ConditionValue = ConditionResult.DontCare;
                }
                else
                {
                    ConditionValue = ConditionEvaluate.CheckCondition(Condition, request);
                }

                Console.WriteLine("\nTarget evaluation: {0}", TargetValue.ToString());
                Console.WriteLine("Condition evaluation: {0}", ConditionValue.ToString());

                // RULE EVALUACIJA

                if (TargetValue == TargetResult.Match && ConditionValue == ConditionResult.True)
                {
                    return Effect;
                }
                else if (TargetValue == TargetResult.Match && ConditionValue == ConditionResult.False)
                {
                    return DecisionType.NotApplicable;
                }
                else if (TargetValue == TargetResult.Match && ConditionValue == ConditionResult.Indeterminate)
                {
                    return DecisionType.Indeterminate;
                }
                else if (TargetValue == TargetResult.NoMatch && ConditionValue == ConditionResult.DontCare)
                {
                    return DecisionType.NotApplicable;
                }
                else if (TargetValue == TargetResult.Indeterminate && ConditionValue == ConditionResult.DontCare)
                {
                    return DecisionType.Indeterminate;
                }
                else
                {
                    return DecisionType.Indeterminate;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                return DecisionType.Indeterminate;
            }
        }
    }
}
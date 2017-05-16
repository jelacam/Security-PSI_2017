using System;
using System.Xml;

namespace PolicyDecisionPoint.XAML_Common
{
    public static class PolicyEvaluateManager
    {
        public const string NO_MATCH = "noMatch";
        public const string MATCH = "match";
        public const string INDETERMINATE = "indeterminate";

        /// <summary>
        ///     Metoda vrsi evaluaciju po principu definisanim XACML standardom: Sekcija C.8 First-applicable
        /// </summary>
        /// <param name="request"></param>
        /// <param name="rule"></param>
        /// <returns></returns>
        public static DecisionType FirstApplRuleEvaluate(RequestType request, RuleType rule)
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

            DecisionType TargetValue = DecisionType.NotApplicable;

            bool ConditionValue = false;

            TargetType Target = rule.Target;

            /// prazan target se poklapa sa svakim request contextom
            if (Target == null)
            {
                return  Effect;
            }

            if (Target.AnyOf == null)
            {
                return  Effect;
            }

            try
            {
                int numberOfMatchAnyOf = 0;
                int numberOfNoMatchAnyOf = 0;
                int numberOfIndeterminateAnyOf = 0;

                if (Target != null)
                {
                    if (Target.AnyOf != null)
                    {
                        foreach (AnyOfType AnyOf in Target.AnyOf)
                        {
                            int numberOfMatchAllOf = 0;
                            int numberOfIndeterminateAllOf = 0;
                            int numberOfNoMatchAllOf = 0;

                            AllOfType[] AllOfs = AnyOf.AllOf;
                            foreach (AllOfType AllOf in AllOfs)
                            {
                                int numberOfFalseMatch = 0;
                                int numberOfIndeterminateMatch = 0;

                                MatchType[] Matches = AllOf.Match;
                                foreach (MatchType Match in Matches)
                                {
                                    AttributeDesignatorType AttributeDesignator = Match.Item as AttributeDesignatorType;
                                    AttributeValueType AttributeValue = Match.AttributeValue;

                                    // Evaluacija Match elementa prema string-equal funkciji
                                    if (Match.MatchId.Equals("urn:oasis:names:tc:xacml:1.0:function:string-equal"))
                                    {
                                        CheckResult decision = CheckIfMatchStringEqual(AttributeValue, AttributeDesignator, request);

                                        if (decision.Equals(CheckResult.False))
                                        {
                                            numberOfFalseMatch++;
                                        }
                                        else if (decision.Equals(CheckResult.Indeterminate))
                                        {
                                            numberOfIndeterminateMatch++;
                                        }
                                    }
                                }

                                /// AllOf evaluacija
                                if (numberOfFalseMatch != 0)
                                {
                                    AllOfValue = NO_MATCH;
                                    numberOfNoMatchAllOf++;
                                }
                                else if (numberOfIndeterminateMatch > 0)
                                {
                                    AllOfValue = INDETERMINATE;
                                    numberOfIndeterminateAllOf++;
                                }
                                else if (numberOfFalseMatch == 0 && numberOfIndeterminateMatch == 0)
                                {
                                    AllOfValue = MATCH;
                                    numberOfMatchAllOf++;
                                }
                            }

                            /// AnyOf evaluacija
                            if (numberOfIndeterminateAllOf > 0 && numberOfMatchAllOf == 0)
                            {
                                AnyOfValue = INDETERMINATE;
                                numberOfIndeterminateAnyOf++;
                            }
                            else if (numberOfMatchAllOf > 0)
                            {
                                AnyOfValue = MATCH;
                                numberOfMatchAnyOf++;
                            }
                            else if (numberOfNoMatchAllOf > 0)
                            {
                                AnyOfValue = NO_MATCH;
                                numberOfNoMatchAnyOf++;
                            }
                        }
                    }
                }

                //// CONDITION EVALUACIJA
                //if (rule.Condition == null)
                //{
                //    ConditionValue = true;
                //}
                // RULE EVALUACIJA

                if (numberOfNoMatchAnyOf > 0)
                {
                    return DecisionType.NotApplicable;
                }
                else if (numberOfMatchAnyOf > 0 && numberOfIndeterminateAnyOf == 0)
                {
                    return  Effect;
                }
                else
                {
                    return  DecisionType.Indeterminate;
                }

              
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                return DecisionType.Indeterminate;
            }
        }

        /// <summary>
        ///     Evaluacija request context-a prema Match elementu
        /// </summary>
        /// <param name="attributeValue">
        ///             Vrednost sa kojom se proverava
        /// </param>
        /// <param name="attributeDesignator">
        ///             Definise tip podataka koji treba da se proveravaju - pravi se bag of attributes
        /// </param>
        /// <param name="request"></param>
        /// <returns></returns>
        private static CheckResult CheckIfMatchStringEqual(AttributeValueType attributeValue,
                                                           AttributeDesignatorType attributeDesignator,
                                                           RequestType request)
        {
            try
            {
                bool exist = false;

                /// atributi zahteva
                AttributesType[] Attributes = request.Attributes;
                foreach (AttributesType Attribute in Attributes)
                {
                    /// provera jednakosti Category atributa
                    if (attributeDesignator.Category.Equals(Attribute.Category))
                    {
                        exist = true;
                        AttributeType[] AttributesType = Attribute.Attribute;

                        foreach (AttributeType AttrType in AttributesType)
                        {
                            /// provera jednakosti AttributeId atributa
                            if (attributeDesignator.AttributeId.Equals(AttrType.AttributeId))
                            {
                                AttributeValueType[] AttributeValues = AttrType.AttributeValue;
                                foreach (AttributeValueType AttrValue in AttributeValues)
                                {
                                    if (AttrValue.DataType.Equals(attributeValue.DataType))
                                    {
                                        XmlNode[] node = AttrValue.Any as XmlNode[];
                                        string value = node[0].Value;

                                        XmlNode[] nodeAttr = attributeValue.Any as XmlNode[];
                                        string valueAttr = nodeAttr[0].Value;

                                        if (value.Equals(valueAttr))
                                        {
                                            return CheckResult.True;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (!exist)
                {
                    /// je bag of attributes prazan
                    /// provera MustBePrestented atributa
                    if (attributeDesignator.MustBePresent)
                    {
                        return CheckResult.Indeterminate;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                return CheckResult.Indeterminate;
            }

            return CheckResult.False;
        }

        private static CheckResult ConditionEvaluation()
        {
            return CheckResult.Indeterminate;
        }
    }
}
using System;
using System.Xml;

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

            /// atributi pravila
            TargetType Target = rule.Target;

            /// prazan target se poklapa sa svakim request contextom
            if (Target.AnyOf.Equals(null))
            {
                return Effect;
            }

            try
            {
                foreach (AnyOfType AnyOf in Target.AnyOf)
                {
                    AllOfType[] AllOfs = AnyOf.AllOf;
                    foreach (AllOfType AllOf in AllOfs)
                    {
                        MatchType[] Matches = AllOf.Match;
                        foreach (MatchType Match in Matches)
                        {
                            AttributeDesignatorType AttributeDesignator = Match.Item as AttributeDesignatorType;
                            AttributeValueType AttributeValue = Match.AttributeValue;

                            if (Match.MatchId.Equals("urn:oasis:names:tc:xacml:1.0:function:string-equal"))
                            {
                                CheckResult decision = CheckIfMatchStringEqual(AttributeValue, AttributeDesignator, request);

                                if (decision.Equals(CheckResult.False))
                                {
                                    return DecisionType.NotApplicable;
                                }
                                else if (decision.Equals(CheckResult.Indeterminate))
                                {
                                    return DecisionType.Indeterminate;
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                return DecisionType.Indeterminate;
            }


            return Effect;
        }

        private static CheckResult CheckIfMatchStringEqual(AttributeValueType attributeValue,
                                                 AttributeDesignatorType attributeDesignator,
                                                 RequestType request)
        {
            try
            {
                /// atributi zahteva
                AttributesType[] Attributes = request.Attributes;
                foreach (AttributesType Attribute in Attributes)
                {
                    if (attributeDesignator.Category.Equals(Attribute.Category))
                    {
                        //ako je kategorija jednaka proveravamo za atribute te kategorije
                        AttributeType[] AttributesType = Attribute.Attribute;
                        foreach (AttributeType AttrType in AttributesType)
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
            catch(Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                return CheckResult.Indeterminate;
            }

            return CheckResult.False;
        }
    }
}
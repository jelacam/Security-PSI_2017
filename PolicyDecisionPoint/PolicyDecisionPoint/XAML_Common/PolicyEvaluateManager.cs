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
            /// atributi pravila

            TargetType Target = rule.Target;
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

                        DecisionType decision = CheckIfMatch(AttributeDesignator, AttributeValue, request);

                        if (!decision.Equals(DecisionType.Permit))
                        {
                            return decision;
                        }
                    }
                }
            }

            return DecisionType.Permit;
        }

        private static DecisionType CheckIfMatch(AttributeDesignatorType attributeDesignator,
                                           AttributeValueType attributeValue,
                                           RequestType request)
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

                                XmlNode[] nodea = attributeValue.Any as XmlNode[];
                                string valuea = nodea[0].Value;

                                if (!value.Equals(valuea))
                                {
                                    return DecisionType.Deny;
                                }
                            }
                            else
                            {
                                return DecisionType.Indeterminate;
                            }
                        }
                    }
                }
            }

            return DecisionType.Permit;
        }
    }
}
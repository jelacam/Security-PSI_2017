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
            
            
            /// atributi pravila
            TargetType Target = rule.Target;

            /// prazan target se poklapa sa svakim request contextom
            if (Target.AnyOf.Equals(null))
            {
                return Effect;
            }

            try
            {
                int numberOfMatchAnyOf = 0;
                int numberOfNoMatchAnyOf = 0;
                int numberOfIndeterminateAnyOf = 0;

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

                            if (Match.MatchId.Equals("urn:oasis:names:tc:xacml:1.0:function:string-equal"))
                            {
                                CheckResult decision = CheckIfMatchStringEqual(AttributeValue, AttributeDesignator, request);

                                if (decision.Equals(CheckResult.False))
                                {
                                    //return DecisionType.NotApplicable;
                                    numberOfFalseMatch++;
                                }
                                else if (decision.Equals(CheckResult.Indeterminate))
                                {
                                    //return DecisionType.Indeterminate;
                                    numberOfIndeterminateMatch++;
                                }
                            }
                        }

                        if (numberOfFalseMatch!=0)
                        {
                            AllOfValue = NO_MATCH;
                            numberOfNoMatchAllOf++;
                        }
                        
                        else if (numberOfFalseMatch==0 && numberOfIndeterminateMatch>0)
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

                    if (numberOfIndeterminateAllOf > 0 && numberOfMatchAllOf == 0)
                    {
                        AnyOfValue = INDETERMINATE;
                        numberOfIndeterminateAnyOf++;
                    }

                    else if (numberOfNoMatchAllOf > 0 && numberOfIndeterminateAllOf == 0)
                    {
                        AnyOfValue = NO_MATCH;
                        numberOfNoMatchAnyOf++;
                        
                    }

                    else if (numberOfMatchAllOf > 0)
                    {
                        AnyOfValue = MATCH;
                        numberOfMatchAnyOf++;
                    }

                    //if (AnyOfValue.Equals(NO_MATCH))
                    //{
                    //    return DecisionType.NotApplicable;
                    //}
                }

                
                if (numberOfMatchAnyOf > 0 && numberOfIndeterminateAnyOf == 0)
                {
                    return Effect;
                }
                else if (numberOfNoMatchAnyOf > 0)
                {
                    return DecisionType.NotApplicable;
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
                    if (attributeDesignator.Category.Equals(Attribute.Category))
                    {
                        //ako je kategorija jednaka proveravamo za atribute te kategorije
                        exist = true;
                        AttributeType[] AttributesType = Attribute.Attribute;

                        foreach (AttributeType AttrType in AttributesType)
                        {
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
                    if(attributeDesignator.MustBePresent)
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
    }
}
using PolicyDecisionPoint.XACML_Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PolicyDecisionPoint.XAML_Common
{
    public static class TargetEvaluate
    {

        public static TargetResult CheckTarget(TargetType Target, RequestType request)
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

                                    List<AttributeType> Attributes = AttributeDesignatorManager.RequestBagOfValues(AttributeDesignator, request);

                                    if (Attributes.Count == 0)
                                    {
                                        // bag of values je prazan, provera atributa MustBePresented
                                        if (AttributeDesignator.MustBePresent)
                                        {
                                            // TODO zahteva dobavljanje atributa od PIP
                                            numberOfIndeterminateMatch++;
                                        }

                                    }
                                    string attributeValue = string.Empty;

                                    foreach(AttributeType attr in Attributes)
                                    {
                                        AttributeValueType[] attrValues = attr.AttributeValue;
                                        foreach(AttributeValueType attrValue in attrValues)
                                        {
                                            XmlNode node = attrValue.Any[0];
                                            attributeValue = node.Value;
                                        }
                                        
                                    }


                                    bool decision = StringEqual.CheckIfMatch(attributeValue, AttributeValue.Any[0].Value);

                                    if (!decision)
                                    {
                                        numberOfFalseMatch++;
                                    }
                                    
                                }
                            }

                            /// AllOf evaluacija
                            if (numberOfFalseMatch != 0)
                            {
                                //AllOfValue = NO_MATCH;
                                numberOfNoMatchAllOf++;
                            }
                            else if (numberOfIndeterminateMatch > 0)
                            {
                                //AllOfValue = INDETERMINATE;
                                numberOfIndeterminateAllOf++;
                            }
                            else if (numberOfFalseMatch == 0 && numberOfIndeterminateMatch == 0)
                            {
                                //AllOfValue = MATCH;
                                numberOfMatchAllOf++;
                            }
                        }

                        /// AnyOf evaluacija
                        if (numberOfIndeterminateAllOf > 0 && numberOfMatchAllOf == 0)
                        {
                            //AnyOfValue = INDETERMINATE;
                            numberOfIndeterminateAnyOf++;
                        }
                        else if (numberOfMatchAllOf > 0)
                        {
                            //AnyOfValue = MATCH;
                            numberOfMatchAnyOf++;
                        }
                        else if (numberOfNoMatchAllOf > 0)
                        {
                            //AnyOfValue = NO_MATCH;
                            numberOfNoMatchAnyOf++;
                        }
                    }
                }
                else
                {
                    // empty target 
                    return TargetResult.Match;
                }
            }
            else
            {
                // empty target
                return TargetResult.Match;
            }


            if (numberOfNoMatchAnyOf > 0)
            {
                return TargetResult.NoMatch;
            }
            else if (numberOfMatchAnyOf > 0 && numberOfIndeterminateAnyOf == 0)
            {
                return TargetResult.Match;
            }
            else
            {
                return TargetResult.Indeterminate;
            }

        }

    }
}

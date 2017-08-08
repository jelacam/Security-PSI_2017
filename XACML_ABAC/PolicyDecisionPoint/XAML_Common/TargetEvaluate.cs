using Contracts;
using PolicyDecisionPoint.XACML_Functions;
using PolicyDecisionPoint.XACML_Match;
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
        public static Dictionary<string, MatchEvaluation> MatchFunctions = new Dictionary<string, MatchEvaluation>(3);

        public static TargetResult CheckTarget(TargetType Target, RequestType request)
        {
            MatchFunctions[XacmlFunctions.STRING_EQUAL] = new StringEqual();

            ContextHandler ch = new ContextHandler();

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
                            try
                            {
                                foreach (MatchType Match in Matches)
                                {
                                    AttributeDesignatorType AttributeDesignator = Match.Item as AttributeDesignatorType;
                                    AttributeValueType AttributeValue = Match.AttributeValue;

                                    List<AttributeType> Attributes = AttributeDesignatorManager.RequestBagOfValues(AttributeDesignator, request);

                                    int numberOfMatch = 0;

                                    if (Attributes.Count == 0)
                                    {
                                        // bag of values je prazan, provera atributa MustBePresented
                                        if (AttributeDesignator.MustBePresent)
                                        {
                                            // TODO zahteva dobavljanje atributa od PIP
                                            if (AttributeDesignator.Category.Equals(XacmlSubject.CATEGORY))
                                            {
                                                string subjectId = SubjectIdResolver.SubjectId(request);
                                                if (!(subjectId == null))
                                                {
                                                    Attributes = ch.RequestForSubjectAttribute(AttributeDesignator, subjectId);
                                                }
                                                else
                                                {
                                                    Attributes = null;
                                                }
                                            }
                                            else
                                            {
                                                Attributes = ch.RequestForEnvironmentAttribute(AttributeDesignator);
                                            }

                                            // ako PIP ne vrati atribut - zbog true vrednosti MustBePresented
                                            if (Attributes != null)
                                            {
                                                if (Attributes[0] == null)
                                                {
                                                    numberOfIndeterminateMatch++;
                                                    continue;
                                                }
                                            }
                                            else
                                            {
                                                numberOfIndeterminateMatch++;
                                                continue;
                                            }
                                            if (Attributes.Count == 0)
                                            {
                                                numberOfIndeterminateMatch++;
                                                continue;
                                            }
                                        }
                                    }
                                    string attributeValue = string.Empty;

                                    foreach (AttributeType attr in Attributes)
                                    {
                                        AttributeValueType[] attrValues = attr.AttributeValue;
                                        foreach (AttributeValueType attrValue in attrValues)
                                        {
                                            XmlNode node = attrValue.Any[0];
                                            attributeValue = node.Value;
                                        }
                                        string value = AttributeValue.Any[0].Value.ToString();

                                        // evaluacija prema funkciji definisanoj MatchId atributom
                                        bool decision = MatchFunctions[Match.MatchId].CheckIfMatch(ref value, ref attributeValue);

                                        if (decision)
                                        {
                                            numberOfMatch++;
                                        }
                                    }

                                    if (numberOfMatch == 0)
                                    {
                                        numberOfFalseMatch++;
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                numberOfIndeterminateMatch++;
                            }

                            /// AllOf evaluacija
                            if (numberOfFalseMatch != 0)
                            {
                                numberOfNoMatchAllOf++;
                            }
                            else if (numberOfIndeterminateMatch > 0)
                            {
                                numberOfIndeterminateAllOf++;
                            }
                            else if (numberOfFalseMatch == 0 && numberOfIndeterminateMatch == 0)
                            {
                                numberOfMatchAllOf++;
                            }
                        }

                        /// AnyOf evaluacija
                        if (numberOfIndeterminateAllOf > 0 && numberOfMatchAllOf == 0)
                        {
                            numberOfIndeterminateAnyOf++;
                        }
                        else if (numberOfMatchAllOf > 0)
                        {
                            numberOfMatchAnyOf++;
                        }
                        else if (numberOfNoMatchAllOf > 0)
                        {
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
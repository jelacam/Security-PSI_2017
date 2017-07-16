using PolicyDecisionPoint.XACML_Functions;
using System;
using System.Xml;

namespace PolicyDecisionPoint.XAML_Common
{
    public static class ConditionEvaluate
    {
        public static ConditionResult CheckCondition(ConditionType Condition, RequestType request)
        {
            ConditionResult Result = ConditionResult.DontCare;

            ApplyType Item = Condition.Item as ApplyType;
            if (Item.FunctionId.Equals("urn:oasis:names:tc:xacml:2.0:function:time-in-range"))
            {
                bool TimeConditionResult = false;

                ApplyType currentTimeItem = Item.Items[0] as ApplyType;
                ApplyType lowerBoundItem = Item.Items[1] as ApplyType;
                ApplyType upperBoundItem = Item.Items[2] as ApplyType;

                // designator za def DataType, Category, AttributeId
                AttributeDesignatorType attributeDesignator = currentTimeItem.Items[0] as AttributeDesignatorType;

                // lower bound time
                ExpressionType[] lowItems = lowerBoundItem.Items;
                ApplyType lowItem = lowItems[0] as ApplyType;
                AttributeValueType lowItemValue = lowItem.Items[0] as AttributeValueType;
                XmlNode lowItemValueAny = lowItemValue.Any[0] as XmlNode;

                string lowerBoundTime = lowItemValueAny.Value as string;

                // upper bound time
                ExpressionType[] uppItems = upperBoundItem.Items;
                ApplyType uppItem = uppItems[0] as ApplyType;
                AttributeValueType uppItemValue = uppItem.Items[0] as AttributeValueType;
                XmlNode uppItemValueAny = uppItemValue.Any[0] as XmlNode;

                string upperBoundTime = uppItemValueAny.Value as string;

                // konverzija vremena - daylight saving time - +1 na vremensku zonu tako da je srbija na +2 po letnjem racunanju vremena
                DateTime lowerBoundTimeValue = DateTime.Parse(lowerBoundTime, System.Globalization.CultureInfo.CurrentCulture);
                DateTime upperBoundTimeValue = DateTime.Parse(upperBoundTime, System.Globalization.CultureInfo.CurrentCulture);
                DateTime currentTime = new DateTime();

                bool exits = false;
                /// atributi zahteva
                AttributesType[] Attributes = request.Attributes;
                foreach (AttributesType Attribute in Attributes)
                {
                    /// provera jednakosti Category atributa
                    if (attributeDesignator.Category.Equals(Attribute.Category))
                    {
                        AttributeType[] AttributesType = Attribute.Attribute;

                        foreach (AttributeType AttrType in AttributesType)
                        {
                            /// provera jednakosti AttributeId atributa
                            if (attributeDesignator.AttributeId.Equals(AttrType.AttributeId))
                            {
                                AttributeValueType[] AttributeValues = AttrType.AttributeValue;
                                foreach (AttributeValueType AttrValue in AttributeValues)
                                {
                                    if (AttrValue.DataType.Equals(attributeDesignator.DataType))
                                    {
                                        XmlNode[] node = AttrValue.Any as XmlNode[];
                                        string value = node[0].Value;

                                        currentTime = DateTime.Parse(value, System.Globalization.CultureInfo.CurrentCulture);

                                        TimeConditionResult = TimeInRange.CheckIfMatch(currentTime, lowerBoundTimeValue, upperBoundTimeValue);
                                        exits = true;
                                    }
                                }
                            }
                        }
                    }
                }

                if (!exits)
                {
                    // exist je false
                    Result = ConditionResult.Indeterminate;
                }
                else if (TimeConditionResult)
                {
                    Result = ConditionResult.True;
                }
                else if (!TimeConditionResult)
                {
                    Result = ConditionResult.False;
                }
                else
                {
                    Result = ConditionResult.DontCare;
                }
            }

            return Result;
        }
    }
}
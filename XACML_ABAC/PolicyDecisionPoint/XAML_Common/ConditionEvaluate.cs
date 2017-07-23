using PolicyDecisionPoint.XACML_Functions;
using System;
using System.Collections.Generic;
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

                bool exists = false;
                List<AttributeType> Attributes = new List<AttributeType>(2);

                Attributes = AttributeDesignatorManager.RequestBagOfValues(attributeDesignator, request);

                if (Attributes.Count == 0)
                {
                    // PDP zahteva od PIP dobavljanje atributa koji su potrebni
                    ContextHandler ch = new ContextHandler();
                    Attributes = ch.RequestForEnvironmentAttribute(attributeDesignator);
                }

                TimeConditionResult = TimeConditionEvaluation(lowerBoundTimeValue, upperBoundTimeValue, Attributes, out exists);

                if (!exists)
                {
                    // ovo se nece desiti
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

        private static bool TimeConditionEvaluation(DateTime lowerBoundTimeValue, DateTime upperBoundTimeValue, List<AttributeType> Attributes, out bool exists)
        {
            foreach (AttributeType attr in Attributes)
            {
                AttributeValueType[] AttrValues = attr.AttributeValue;

                foreach (AttributeValueType AttrValue in AttrValues)
                {
                    XmlNode[] node = AttrValue.Any as XmlNode[];
                    string value = node[0].Value;

                    DateTime currentTime = DateTime.Parse(value, System.Globalization.CultureInfo.CurrentCulture);
                    exists = true;
                    return TimeInRange.CheckIfMatch(currentTime, lowerBoundTimeValue, upperBoundTimeValue);
                }
            }

            exists = false;
            return false;
        }
    }
}
using Common;
using PolicyDecisionPoint.XACML_Condition;
using PolicyDecisionPoint.XACML_Functions;
using System;
using System.Collections.Generic;
using System.Xml;

namespace PolicyDecisionPoint.XAML_Common
{
    public static class ConditionEvaluate
    {
        public static Dictionary<string, ConditionAttributes> ConditionEvaluationFor = new Dictionary<string, ConditionAttributes>();

        public static ConditionResult CheckCondition(ConditionType Condition, RequestType request)
        {
            ConditionEvaluationFor[XacmlFunctions.TIME_IN_RANGE] = new TimeInRangeCondition();

            ConditionResult Result = XAML_Common.ConditionResult.DontCare;

            if (Condition == null)
            {
                return XAML_Common.ConditionResult.True;
            }

            ApplyType Item = Condition.Item as ApplyType;

            bool ConditionResult = ConditionEvaluationFor[Item.FunctionId].EvaluateConditionFor(Item, request);

            if (ConditionResult)
            {
                Result = XAML_Common.ConditionResult.True;
            }
            else if (!ConditionResult)
            {
                Result = XAML_Common.ConditionResult.False;
            }
            else
            {
                Result = XAML_Common.ConditionResult.DontCare;
            }

            return Result;
        }
    }
}
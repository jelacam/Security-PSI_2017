using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolicyDecisionPoint.XACML_Condition
{
    public abstract class ConditionAttributes
    {
        public virtual bool EvaluateConditionFor(ApplyType Item, RequestType request)
        {
            return true;
        }
    }
}
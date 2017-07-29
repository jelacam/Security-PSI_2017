using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolicyDecisionPoint.XACML_CombAlg
{
    public abstract class PolicyCombiningAlg
    {
        public virtual DecisionType Evaluate(PolicyType[] policies, RequestType request)
        {
            return DecisionType.NotApplicable;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolicyDecisionPoint.XACML_CombAlg
{
    public abstract class RuleCombiningAlg
    {
        public virtual DecisionType Evaluate(RuleType[] rules, RequestType request)
        {
            return DecisionType.NotApplicable;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolicyDecisionPoint.XACML_Functions
{
    public static class TimeInRange
    {
        public static bool CheckIfMatch(DateTime currentTime, DateTime lowerBound, DateTime upperBoud)
        {
            if (currentTime >= lowerBound && currentTime <= upperBoud)
            {
                return true;
            }
            else
            {
                return false;
            }
       
        }
    }
}

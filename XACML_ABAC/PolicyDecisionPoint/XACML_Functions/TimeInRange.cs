using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolicyDecisionPoint.XACML_Functions
{
    public static class TimeInRange
    {
        public static bool CheckIfMatch(DateTime currentTime, DateTime lowerBound, DateTime upperBound)
        {
            if (currentTime >= lowerBound && currentTime <= upperBound)
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

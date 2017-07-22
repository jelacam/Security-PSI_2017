using PolicyDecisionPoint.XAML_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PolicyDecisionPoint.XACML_Functions
{
    public static class StringEqual
    {
        /// <summary>
        ///     
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static bool CheckIfMatch(string value1, string value2)
        {
            return value1.Equals(value2);
        }
    }
}

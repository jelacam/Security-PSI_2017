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
        ///     Poredjenje dva atirbuta - string-equal
        ///    
        /// </summary>
        /// <param name="value1"> Atribut u Match elementu pravila </param>
        /// <param name="value2"> Atribut u xacml zahtevu dobavljen na osnovu AttributeDesignator elementa </param>
        /// <returns></returns>
        public static bool CheckIfMatch(string value1, string value2)
        {
            return value1.Equals(value2);
        }
    }
}

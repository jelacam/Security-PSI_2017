using PolicyDecisionPoint.XACML_Match;
using PolicyDecisionPoint.XAML_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PolicyDecisionPoint.XACML_Functions
{
    /// <summary>
    ///     Funkcija definisana MatchId atributom
    /// </summary>
    public class StringEqual : MatchEvaluation
    {
        public override bool CheckIfMatch<T>(ref T value1, ref T value2)
        {
            return value1.Equals(value2);
        }
    }
}
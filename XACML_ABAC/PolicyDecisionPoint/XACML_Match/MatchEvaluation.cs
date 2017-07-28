using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolicyDecisionPoint.XACML_Match
{
    public abstract class MatchEvaluation
    {
        /// <summary>
        ///     Vrsi poredjenje vrednosti dva atributa prema definisanoj funkciji
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value1"> Atribut u Match elementu pravila </param>
        /// <param name="value2"> Atribut u xacml zahtevu dobavljen na osnovu AttributeDesignator elementa </param>
        /// <returns></returns>
        public virtual bool CheckIfMatch<T>(ref T value1, ref T value2)
        {
            return true;
        }
    }
}
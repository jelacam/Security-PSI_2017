using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolicyDecisionPoint.XAML_Common
{
    internal static class AttributeConversionManager
    {
        public static readonly Dictionary<string, string> CategoryConversion = new Dictionary<string, string>
        {
            {"action", XacmlAction.CATEGORY },
            {"resource", XacmlResource.CATEGORY},
            {"environment", XacmlEnvironment.CATEGORY},
            {"subject", XacmlSubject.CATEGORY}
        };

        public static readonly Dictionary<string, string> IdConversion = new Dictionary<string, string>
        {
            {"action-id", XacmlAction.ID},
            {"resource-id", XacmlResource.ID},
            {"subject-id", XacmlSubject.ID},
            {"subject-role", XacmlSubject.ROLE},
            {"current-time", XacmlEnvironment.CURRENT_TIME_ID},
            {"location", XacmlEnvironment.CURRENT_LOCATION_ID}
        };

        public static readonly Dictionary<string, string> DataTypeConversion = new Dictionary<string, string>()
        {
            {"string", XacmlDataTypes.STRING},
            {"time", XacmlDataTypes.TIME},
            {"boolean", XacmlDataTypes.BOOLEAN},
            {"integer", XacmlDataTypes.INTEGER},
            {"double", XacmlDataTypes.DOUBLE}
        };
    }
}
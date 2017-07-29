namespace Contracts
{
    public static class XacmlAction
    {
        /// <summary>
        /// Category
        /// </summary>
        public const string CATEGORY = "urn:oasis:names:tc:xacml:3.0:attribute-category:action";

        /// <summary>
        /// This attribute identifies the action for which access is requested
        /// </summary>
        public const string ID = "urn:oasis:names:tc:xacml:1.0:action:action-id";
    }

    public static class XacmlResource
    {
        /// <summary>
        /// Category
        /// </summary>
        public const string CATEGORY = "urn:oasis:names:tc:xacml:3.0:attribute-category:resource";

        /// <summary>
        /// This attribute identifies the resource to which access is requested
        /// </summary>
        public const string ID = "urn:oasis:names:tc:xacml:1.0:resource:resource-id";
    }

    public static class XacmlSubject
    {
        /// <summary>
        /// Category
        /// </summary>
        public const string CATEGORY = "urn:oasis:names:tc:xacml:3.0:subject-category:access-subject";

        /// <summary>
        /// This identifier indicates the name of the subject
        /// </summary>
        public const string ID = "urn:oasis:names:tc:xacml:1.0:subject:subject-id";

        public const string LOCATION = "urn:oasis:names:tc:xacml:1.0:subject:subject-location";
    }

    public static class XacmlEnvironment
    {
        /// <summary>
        /// Category
        /// </summary>
        public const string CATEGORY = "urn:oasis:names:tc:xacml:3.0:attribute-category:environment";

        /// <summary>
        ///  This identifier indicates current time
        /// </summary>
        public const string CURRENT_TIME = "urn:oasis:names:tc:xacml:1.0:environment:current-time";
    }

    public static class XacmlDataTypes
    {
        public const string STRING = "http://www.w3.org/2001/XMLSchema#string";
        public const string TIME = "http://www.w3.org/2001/XMLSchema#time";

        public const string BOOLEAN = "http://www.w3.org/2001/XMLSchema#boolean";
        public const string INTEGER = "http://www.w3.org/2001/XMLSchema#integer";
        public const string DOUBLE = "http://www.w3.org/2001/XMLSchema#double";
    }

    public static class XacmlFunctions
    {
        public const string STRING_EQUAL = "urn:oasis:names:tc:xacml:1.0:function:string-equal";
        public const string TIME_IN_RANGE = "urn:oasis:names:tc:xacml:2.0:function:time-in-range";
    }

    public static class XacmlRuleCombAlg
    {
        public const string FIRST_APPLICABLE = "urn:oasis:names:tc:xacml:1.0:rule-combining-algorithm:first-applicable";
    }

    public static class XacmlPolicyCombAlg
    {
        public const string FIRST_APPLICABLE = "urn:oasis:names:tc:xacml:1.0:policy-combining-algorithm:first-applicable";
    }
}
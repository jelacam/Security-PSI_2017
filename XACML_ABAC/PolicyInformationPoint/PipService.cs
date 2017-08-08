using Contracts;
using Contracts.Contracts;
using PolicyInformationPoint.Environment;
using PolicyInformationPoint.Subject;
using System;
using System.Collections.Generic;

namespace PolicyInformationPoint
{
    public class PipService : IPipContract
    {
        public static Dictionary<string, EnvironmentAttributes> GetEnvironmentAttribute = new Dictionary<string, EnvironmentAttributes>(3);
        //public static Dictionary<string, SubjectAttributes> GetSubjectAttribute = new Dictionary<string, SubjectAttributes>(3);

        public PipService()
        {
            GetEnvironmentAttribute[XacmlEnvironment.CURRENT_TIME_ID] = new CurrentTime();
        }

        public DomainAttribute RequestEnvironmentAttribute(string AttributeId)
        {
            DomainAttribute DomainAttr = null;

            try
            {
                EnvironmentAttributes reference = null;
                if (!GetEnvironmentAttribute.TryGetValue(AttributeId, out reference))
                {
                    return new DomainAttribute();
                }

                DomainAttr = reference.RequestForEnvironmentAttributes();
            }
            catch (Exception e)
            {
                Console.WriteLine("Request environment attribute error - Message: {0}", e.Message);
            }

            return DomainAttr;
        }

        public List<DomainAttribute> RequestSubjectAttributes(string AttributeId, string SubjectId)
        {
            List<DomainAttribute> Attributes = new List<DomainAttribute>(5);

            try
            {
                HashSet<string> requestedValues = new SubjectAttributes().RequestForSubjectAttributes(SubjectId, AttributeId);

                foreach (string value in requestedValues as HashSet<string>)
                {
                    DomainAttribute attribute = new DomainAttribute();
                    attribute.AttributeId = AttributeId;
                    attribute.DataType = "string";
                    attribute.Value = value;

                    Attributes.Add(attribute);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Request subject attribute error - Message: {0}", e.Message);
            }

            return Attributes;
        }
    }
}
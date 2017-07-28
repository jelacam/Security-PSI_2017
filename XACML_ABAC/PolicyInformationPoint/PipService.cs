using Contracts;
using Contracts.Contracts;
using PolicyInformationPoint.Environment;
using System;
using System.Collections.Generic;

namespace PolicyInformationPoint
{
    public class PipService : IPipContract
    {
        public static Dictionary<string, EnvironmentAttributes> GetEnvironmentAttribute = new Dictionary<string, EnvironmentAttributes>(3);

        public PipService()
        {
            GetEnvironmentAttribute[XacmlEnvironment.CURRENT_TIME] = new CurrentTime();
        }

        public DomainAttribute RequestEnvironmentAttribute(string AttrType)
        {
            DomainAttribute DomainAttr = null;

            DomainAttr = GetEnvironmentAttribute[AttrType].RequestForEnvironmentAttributes();

            Console.WriteLine("PIP - Request for environment attribute - current-time");

            return DomainAttr;
        }
    }
}
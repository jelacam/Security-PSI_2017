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
            GetEnvironmentAttribute[XacmlEnvironment.CURRENT_TIME_ID] = new CurrentTime();
            GetEnvironmentAttribute[XacmlEnvironment.CURRENT_LOCATION_ID] = new CurrentLocation();
        }

        public DomainAttribute RequestEnvironmentAttribute(string AttributeId)
        {
            DomainAttribute DomainAttr = null;

            try
            {
                DomainAttr = GetEnvironmentAttribute[AttributeId].RequestForEnvironmentAttributes();
            }
            catch (Exception e)
            {
                Console.WriteLine("Request environment attribute error - Message: {0}", e.Message);
            }

            return DomainAttr;
        }
    }
}
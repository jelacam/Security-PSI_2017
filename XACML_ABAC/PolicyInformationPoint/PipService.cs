using Contracts;
using Contracts.Contracts;
using PolicyInformationPoint.Environment;
using System;

namespace PolicyInformationPoint
{
    public class PipService : IPipContract
    {
        public DomainAttribute RequestEnvironmentAttribute(string AttrType)
        {
            EnvironmentAttributes EnvAttr = null;

            DomainAttribute DomainAttr = null;

            switch (AttrType)
            {
                case XacmlEnvironment.CURRENT_TIME:
                    {
                        EnvAttr = RequestForCurrentTime();
                        DomainAttr = EnvAttr.RequestForEnvironmentAttributes();
                        DomainAttr.AttributeId = XacmlEnvironment.CURRENT_TIME;
                        DomainAttr.DataType = XacmlDataTypes.TIME;
                        break;
                    }
            }
            DomainAttr.Category = XacmlEnvironment.CATEGORY;
            Console.WriteLine("PIP - Request for environment attribute - current-time");

            return DomainAttr;
        }

        private EnvironmentAttributes RequestForCurrentTime()
        {
            CurrentTime CurrentTime = new CurrentTime();
            return CurrentTime;
        }
    }
}
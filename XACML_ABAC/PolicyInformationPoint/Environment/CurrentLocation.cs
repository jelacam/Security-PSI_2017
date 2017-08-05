using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using System.Security.Principal;
using System.ServiceModel;
using System.Threading;

namespace PolicyInformationPoint.Environment
{
    public class CurrentLocation : EnvironmentAttributes
    {
        public override DomainAttribute RequestForEnvironmentAttributes()
        {
            Console.WriteLine("PIP - Request for environment attribute - subject current location");

            string location = "Novi Sad";

            DomainAttribute locationAttribute = new DomainAttribute();
            locationAttribute.AttributeId = "location";
            locationAttribute.Category = "environment";
            locationAttribute.DataType = "string";
            locationAttribute.Value = location;

            return locationAttribute;
        }
    }
}
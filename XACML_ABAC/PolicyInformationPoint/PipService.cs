using Contracts.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;

namespace PolicyInformationPoint
{
    public class PipService : IPipContract
    {
        public DomainAttribute RequestEnvironmentAttribute(string AttrType)
        {
            DomainAttribute CurrentTimeAttribute = new DomainAttribute();
            CurrentTimeAttribute.AttributeId = "urn:oasis:names:tc:xacml:1.0:environment:current-time";
            CurrentTimeAttribute.Category = "urn:oasis:names:tc:xacml:3.0:attribute-category:environment";
            CurrentTimeAttribute.DataType = "http://www.w3.org/2001/XMLSchema#time";
            CurrentTimeAttribute.Value = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString();

            return CurrentTimeAttribute;

        }
    }
}

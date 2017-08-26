using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PolicyDecisionPoint.XAML_Common
{
    public static class SubjectIdResolver
    {
        public static string SubjectId(RequestType request)
        {
            string subjectId = string.Empty;

            AttributesType[] Attributes = request.Attributes;

            foreach (AttributesType Attribute in Attributes)
            {
                /// provera jednakosti Category atributa
                if (Attribute.Category.Equals(XacmlSubject.CATEGORY))
                {
                    AttributeType[] AttributesType = Attribute.Attribute;

                    foreach (AttributeType AttrType in AttributesType)
                    {
                        /// provera jednakosti AttributeId atributa
                        if (AttrType.AttributeId.Equals(XacmlSubject.ID))
                        {
                            AttributeValueType[] AttributeValues = AttrType.AttributeValue;
                            foreach (AttributeValueType AttrValue in AttributeValues)
                            {
                                if (AttrValue.DataType.Equals(XacmlDataTypes.STRING))
                                {
                                    XmlNode node = AttrValue.Any[0];
                                    subjectId = node.Value;
                                }
                            }
                        }
                    }
                }
            }

            return subjectId;
        }
    }
}
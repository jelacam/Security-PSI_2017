using System.Collections.Generic;

namespace PolicyDecisionPoint
{
    public static class AttributeDesignatorManager
    {
        public static List<AttributeType> RequestBagOfValues(AttributeDesignatorType attributeDesignator, RequestType request)
        {
            List<AttributeType> RequestedAttributes = new List<AttributeType>(3);

            AttributesType[] Attributes = request.Attributes;

            foreach (AttributesType Attribute in Attributes)
            {
                /// provera jednakosti Category atributa
                if (attributeDesignator.Category.Equals(Attribute.Category))
                {
                    AttributeType[] AttributesType = Attribute.Attribute;

                    foreach (AttributeType AttrType in AttributesType)
                    {
                        /// provera jednakosti AttributeId atributa
                        if (attributeDesignator.AttributeId.Equals(AttrType.AttributeId))
                        {
                            AttributeValueType[] AttributeValues = AttrType.AttributeValue;
                            foreach (AttributeValueType AttrValue in AttributeValues)
                            {
                                if (AttrValue.DataType.Equals(attributeDesignator.DataType))
                                {
                                    RequestedAttributes.Add(AttrType);
                                }
                            }
                        }
                    }
                }
            }

            return RequestedAttributes;
        }
    }
}
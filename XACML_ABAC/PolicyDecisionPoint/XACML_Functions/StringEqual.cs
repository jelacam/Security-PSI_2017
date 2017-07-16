using PolicyDecisionPoint.XAML_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PolicyDecisionPoint.XACML_Functions
{
    public static class StringEqual
    {
        /// <summary>
        ///     Evaluacija request context-a prema Match elementu
        /// </summary>
        /// <param name="attributeValue">
        ///             Vrednost sa kojom se proverava
        /// </param>
        /// <param name="attributeDesignator">
        ///             Definise tip podataka koji treba da se proveravaju - pravi se bag of attributes
        /// </param>
        /// <param name="request"></param>
        /// <returns></returns>
        public static CheckResult CheckIfMatch(AttributeValueType attributeValue,
                                                           AttributeDesignatorType attributeDesignator,
                                                           RequestType request)
        {
            try
            {
                bool exist = false;

                /// atributi zahteva
                AttributesType[] Attributes = request.Attributes;
                foreach (AttributesType Attribute in Attributes)
                {
                    /// provera jednakosti Category atributa
                    if (attributeDesignator.Category.Equals(Attribute.Category))
                    {
                        exist = true;
                        AttributeType[] AttributesType = Attribute.Attribute;

                        foreach (AttributeType AttrType in AttributesType)
                        {
                            /// provera jednakosti AttributeId atributa
                            if (attributeDesignator.AttributeId.Equals(AttrType.AttributeId))
                            {
                                AttributeValueType[] AttributeValues = AttrType.AttributeValue;
                                foreach (AttributeValueType AttrValue in AttributeValues)
                                {
                                    if (AttrValue.DataType.Equals(attributeValue.DataType))
                                    {
                                        XmlNode[] node = AttrValue.Any as XmlNode[];
                                        string value = node[0].Value;

                                        XmlNode[] nodeAttr = attributeValue.Any as XmlNode[];
                                        string valueAttr = nodeAttr[0].Value;

                                        if (value.Equals(valueAttr))
                                        {
                                            return CheckResult.True;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (!exist)
                {
                    /// je bag of attributes prazan
                    /// provera MustBePrestented atributa
                    if (attributeDesignator.MustBePresent)
                    {
                        return CheckResult.Indeterminate;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                return CheckResult.Indeterminate;
            }

            return CheckResult.False;
        }
    }
}

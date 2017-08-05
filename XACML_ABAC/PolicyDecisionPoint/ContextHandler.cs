using Contracts;
using Contracts.Contracts;
using PolicyDecisionPoint.XAML_Common;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Xml;

namespace PolicyDecisionPoint
{
    /// <summary>
    ///     ContextHandler omogucava prevodjenje domenskog zahteva u XACML zahtev
    ///     kao i prevodjenje XACML odgovora u domenski odgovor
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class ContextHandler : IContextHandler
    {
        /// <summary>
        ///  Formira XACML zahtev i salje ga PDP komponenti.
        ///  Iz dobijenog XACML odgovora izdvaja samo odluku.
        /// </summary>
        /// <param name="DomainAttributes"></param>
        /// <returns></returns>
        public DecisionType CheckAccess(Dictionary<string, List<DomainAttribute>> DomainAttributes)
        {
            RequestType request = CreateXacmlRequest(DomainAttributes);

            ResponseType response = PdpService.Evaluate(request);

            ResultType[] result = response.Result;

            return result[0].Decision;
        }

        /// <summary>
        ///     Kreira XACML request na osnovu atributa pristiglih iz domenskog zahteva
        /// </summary>
        /// <param name="DomainAttributes"></param>
        private RequestType CreateXacmlRequest(Dictionary<string, List<DomainAttribute>> DomainAttributes)
        {
            RequestType request = new RequestType();
            request.ReturnPolicyIdList = false;

            request.Attributes = new AttributesType[DomainAttributes.Count];
            int attrCount = 0;

            foreach (KeyValuePair<string, List<DomainAttribute>> kvp in DomainAttributes)
            {
                AttributesType Attributes = new AttributesType();

                /// provera da li kategorije ima u dictionary, ako nema kreira se Xacml atribut sa vrednosti
                /// kategorije iz domenskog oblika
                string category = null;
                if (!AttributeConversionManager.CategoryConversion.TryGetValue(kvp.Key, out category))
                {
                    category = kvp.Key;
                }

                Attributes.Category = category;

                Attributes.Attribute = new AttributeType[kvp.Value.Count];
                int index = 0;

                foreach (DomainAttribute attr in kvp.Value)
                {
                    AttributeType AttrType = new AttributeType();
                    AttrType = CreateXacmlAttribute(attr);
                    Attributes.Attribute[index++] = AttrType;
                }

                request.Attributes[attrCount++] = Attributes;
            }

            return request;
        }

        /// <summary>
        ///     Zahteva od PIP komponente dodatne atribute.
        /// </summary>
        /// <param name="attributeDesignator"></param>
        /// <returns></returns>
        public List<AttributeType> RequestForEnvironmentAttribute(AttributeDesignatorType attributeDesignator)
        {
            List<AttributeType> RequestedAttributes = new List<AttributeType>(3);

            /// zahteva od PIP komponente atribute okruzenja
            NetTcpBinding binding = new NetTcpBinding();
            binding.CloseTimeout = new TimeSpan(0, 10, 0);
            binding.OpenTimeout = new TimeSpan(0, 10, 0);
            binding.ReceiveTimeout = new TimeSpan(0, 10, 0);
            binding.SendTimeout = new TimeSpan(0, 10, 0);
            string address = "net.tcp://localhost:7000/PipService";

            DomainAttribute EnvironmentAttribute = new DomainAttribute();

            using (PipProxy proxy = new PipProxy(binding, new EndpointAddress(address)))
            {
                EnvironmentAttribute = proxy.RequestEnvironmentAttribute(attributeDesignator.AttributeId);
            }

            AttributeType XacmlAttribute = CreateXacmlAttribute(EnvironmentAttribute);

            RequestedAttributes.Add(XacmlAttribute);
            return RequestedAttributes;
        }

        /// <summary>
        ///     Kreira XACML atribut na osnovu domenskog atributa
        /// </summary>
        /// <param name="DomainAttribute"></param>
        /// <returns></returns>
        private AttributeType CreateXacmlAttribute(DomainAttribute DomainAttribute)
        {
            if (DomainAttribute.AttributeId == null || DomainAttribute.DataType == null)
            {
                return null;
            }

            AttributeType AttrType = new AttributeType();
            AttrType.IncludeInResult = false;

            string id = null;
            if (!AttributeConversionManager.IdConversion.TryGetValue(DomainAttribute.AttributeId, out id))
            {
                id = DomainAttribute.AttributeId;
            }

            AttrType.AttributeId = id;

            AttributeValueType AttrValue = new AttributeValueType();

            string dataType = null;
            if (!AttributeConversionManager.DataTypeConversion.TryGetValue(DomainAttribute.DataType, out dataType))
            {
                dataType = DomainAttribute.DataType;
            }

            AttrValue.DataType = dataType;

            XmlDocument doc = new XmlDocument();
            XmlNode[] nodes = new XmlNode[1];
            XmlNode node = doc.CreateNode(XmlNodeType.Text, "Value", "");
            node.Value = DomainAttribute.Value;

            nodes[0] = node;
            AttrValue.Any = nodes;

            AttributeValueType[] attrVals = new AttributeValueType[1];
            attrVals[0] = AttrValue;

            AttrType.AttributeValue = attrVals;

            return AttrType;
        }
    }
}
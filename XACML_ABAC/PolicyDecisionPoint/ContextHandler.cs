using Contracts;
using Contracts.Contracts;
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
        ///     Kreira XACML request na osnovu atributa pristiglih iz domenskog zahteva
        /// </summary>
        /// <param name="DomainAttributes"></param>
        public DecisionType CheckAccess(Dictionary<string, List<DomainAttribute>> DomainAttributes)
        {
            /// zahteva od PIP komponente atribute okruzenja
            //NetTcpBinding binding = new NetTcpBinding();
            //binding.CloseTimeout = new TimeSpan(0, 10, 0);
            //binding.OpenTimeout = new TimeSpan(0, 10, 0);
            //binding.ReceiveTimeout = new TimeSpan(0, 10, 0);
            //binding.SendTimeout = new TimeSpan(0, 10, 0);
            //string address = "net.tcp://localhost:7000/PipService";

            //DomainAttribute CurrentTimeAttribute = new DomainAttribute();

            //using (PipProxy proxy = new PipProxy(binding, new EndpointAddress(address)))
            //{
            //    CurrentTimeAttribute = proxy.RequestCurrentTimeAttribute();
            //}

            //DomainAttributes[CurrentTimeAttribute.Category] = new List<DomainAttribute>() { CurrentTimeAttribute };

            /// kreira xacml request na osnovu pristiglih atributa subjekta, resursa i akcije
            RequestType request = CreateXacmlRequest(DomainAttributes);

            DecisionType decision = PdpService.Evaluate(request);

            return decision;
        }

        private RequestType CreateXacmlRequest(Dictionary<string, List<DomainAttribute>> DomainAttributes)
        {
            RequestType request = new RequestType();
            request.ReturnPolicyIdList = false;

            request.Attributes = new AttributesType[DomainAttributes.Count];
            int attrCount = 0;

            foreach (KeyValuePair<string, List<DomainAttribute>> kvp in DomainAttributes)
            {
                AttributesType Attributes = new AttributesType();
                Attributes.Category = kvp.Key;

                //AttributeType[] Attributes = new AttributeType[kvp.Value.Count];
                Attributes.Attribute = new AttributeType[kvp.Value.Count];
                int index = 0;

                foreach (DomainAttribute attr in kvp.Value)
                {
                    AttributeType AttrType = new AttributeType();
                    //AttrType.IncludeInResult = false;
                    //AttrType.AttributeId = attr.AttributeId;

                    //AttributeValueType AttrValue = new AttributeValueType();
                    //AttrValue.DataType = attr.DataType;

                    //XmlDocument doc = new XmlDocument();
                    //XmlNode[] nodes = new XmlNode[1];
                    //XmlNode node = doc.CreateNode(XmlNodeType.Text, "Value", "");
                    ////node.InnerText = attr.Value;
                    //node.Value = attr.Value;

                    //nodes[0] = node;
                    //AttrValue.Any = nodes;

                    //AttributeValueType[] attrVals = new AttributeValueType[1];
                    //attrVals[0] = AttrValue;

                    //AttrType.AttributeValue = attrVals;

                    AttrType = CreateXacmlAttribute(attr);

                    Attributes.Attribute[index++] = AttrType;
                }

                request.Attributes[attrCount++] = Attributes;
            }

            return request;
        }

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

        private AttributeType CreateXacmlAttribute(DomainAttribute DomainAttribute)
        {
            AttributeType AttrType = new AttributeType();
            AttrType.IncludeInResult = false;
            AttrType.AttributeId = DomainAttribute.AttributeId;

            AttributeValueType AttrValue = new AttributeValueType();
            AttrValue.DataType = DomainAttribute.DataType;

            XmlDocument doc = new XmlDocument();
            XmlNode[] nodes = new XmlNode[1];
            XmlNode node = doc.CreateNode(XmlNodeType.Text, "Value", "");
            //node.InnerText = attr.Value;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace Common
{
    [DataContract]
    public class DomainAttribute
    {
        private string category;
        private string attributeId;
        private string value;
        private string dataType;

        [DataMember]
        public string Category { get => category; set => category = value; }

        [DataMember]
        public string AttributeId { get => attributeId; set => attributeId = value; }

        [DataMember]
        public string Value { get => value; set => this.value = value; }

        [DataMember]
        public string DataType { get => dataType; set => dataType = value; }

        public DomainAttribute() { }
    }
}

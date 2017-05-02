
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XACML_Parser
{
    public class Program
    {
        static void Main(string[] args)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(PolicyType));
            
            StreamReader reader = new StreamReader("rule1.main.xml");
            var value = serializer.Deserialize(reader);
        }
    }
}

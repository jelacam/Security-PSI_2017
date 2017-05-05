using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PolicyDecisionPoint
{
    class Program
    {
        static void Main(string[] args)
        {
            /// deserijalizacija xml dokumenta koji specificira autorizacionu politiku
            XmlSerializer serializer = new XmlSerializer(typeof(PolicyType));
            StreamReader reader = new StreamReader("rule1.main.xml");
            var value = serializer.Deserialize(reader);

            /// kreiranje objekta koji predstavlja definisanu politiku 
            PolicyType policy = new PolicyType();
            policy = value as PolicyType;
        }
    }
}

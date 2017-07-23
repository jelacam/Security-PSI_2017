using Contracts.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PolicyAdministrationPoint
{
    public class PapService : IPapContract
    {
        public PolicyType LoadPolicy()
        {
            /// deserijalizacija xml dokumenta koji specificira autorizacionu politiku
            XmlSerializer serializer = new XmlSerializer(typeof(PolicyType));
            StreamReader reader = new StreamReader("rule1.main.xml");
            //StreamReader reader = new StreamReader("TimeRange.checkTimeInRange.xml");
            var value = serializer.Deserialize(reader);

            /// kreiranje objekta koji predstavlja definisanu politiku 
            PolicyType policy = new PolicyType();
            policy = value as PolicyType;

            return policy;
        }
    }
}

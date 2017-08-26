using Common.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PolicyAdministrationPoint
{
    public class PapService : IPrpContract
    {
        public PolicySetType Load()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(PolicySetType));
            StreamReader reader = new StreamReader("rule1.main.xml");
            //StreamReader reader = new StreamReader("TimeRange.checkTimeInRange.xml");
            var value = serializer.Deserialize(reader);

            /// kreiranje objekta koji predstavlja definisanu politiku
            PolicySetType policy = new PolicySetType();

            try
            {
                policy = value as PolicySetType;
            }
            catch (Exception)
            {
                policy = null;
            }

            Console.WriteLine("Policy set loaded");

            return policy;
        }
    }
}
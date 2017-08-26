using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PolicyInformationPoint.Subject
{
    public class SubjectAttributes
    {
        public HashSet<string> RequestForSubjectAttributes(string subjectId, string attributeId)
        {
            HashSet<string> subjectAttribute = null;
            NetTcpBinding binding = new NetTcpBinding();
            binding.CloseTimeout = new TimeSpan(0, 10, 0);
            binding.OpenTimeout = new TimeSpan(0, 10, 0);
            binding.ReceiveTimeout = new TimeSpan(0, 10, 0);
            binding.SendTimeout = new TimeSpan(0, 10, 0);

            string address = "net.tcp://localhost:5000/SubjectAdministrationService";

            using (SubjectInfProxy proxy = new SubjectInfProxy(binding, new EndpointAddress(new Uri(address))))
            {
                subjectAttribute = proxy.GetUserInformation(subjectId, attributeId);
                Console.WriteLine("PIP - Request for subject attribute: {0}", attributeId.ToString());
            }

            return subjectAttribute;
        }
    }
}
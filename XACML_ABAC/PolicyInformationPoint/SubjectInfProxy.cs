using Contracts.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PolicyInformationPoint
{
    public class SubjectInfProxy : ChannelFactory<ISubjectInformation>, ISubjectInformation, IDisposable
    {
        private ISubjectInformation factory;

        public SubjectInfProxy(NetTcpBinding binding, EndpointAddress address)
            : base(binding, address)
        {
            factory = CreateChannel();
        }

        public HashSet<string> GetUserInformation(string userId, string attributeId)
        {
            try
            {
                return factory.GetUserInformation(userId, attributeId);
            }
            catch (Exception e)
            {
                Console.WriteLine("Subject information proxy error. Message: {0}", e.Message);
                return null;
            }
        }

        public void Dispose()
        {
            if (factory != null)
            {
                factory = null;
            }

            this.Close();
        }
    }
}
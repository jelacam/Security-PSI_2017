using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Contracts.Contracts;
using Contracts;

namespace PolicyDecisionPoint
{
    public class PipProxy : ChannelFactory<IPipContract>, IPipContract, IDisposable
    {
        private IPipContract factory;

        public PipProxy(NetTcpBinding binding, EndpointAddress address)
            : base(binding, address)
        {
            factory = CreateChannel();
        }

        public DomainAttribute RequestEnvironmentAttribute(string AttrType)
        {
            try
            {
                return factory.RequestEnvironmentAttribute(AttrType);
            }
            catch (Exception e)
            {
                Console.WriteLine("PipProxy error: Message: {0}", e.Message);
                return null;
            }
        }

        public List<DomainAttribute> RequestSubjectAttributes(string AttrType, string subjectId)
        {
            try
            {
                return factory.RequestSubjectAttributes(AttrType, subjectId);
            }
            catch (Exception e)
            {
                Console.WriteLine("PipProxy error: Message: {0}", e.Message);
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
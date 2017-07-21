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
        public DomainAttribute RequestCurrentTimeAttribute()
        {
            try
            {
                return factory.RequestCurrentTimeAttribute();
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

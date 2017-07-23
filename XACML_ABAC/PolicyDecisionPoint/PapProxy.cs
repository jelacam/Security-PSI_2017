using Contracts.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PolicyDecisionPoint
{
    public class PapProxy : ChannelFactory<IPapContract>, IPapContract, IDisposable
    {
        IPapContract factory;

        public PapProxy(NetTcpBinding binding, EndpointAddress address)
            : base(binding, address)
        {
            factory = CreateChannel();
        }
        public PolicyType LoadPolicy()
        {
            try
            {
                return factory.LoadPolicy();
            }
            catch (Exception e)
            {

                Console.WriteLine("PapProxy error: Message: {0}", e.Message);
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

using Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PolicyDecisionPoint
{
    public class PapProxy : ChannelFactory<IPrpContract>, IPrpContract, IDisposable
    {
        private IPrpContract factory;

        public PapProxy(NetTcpBinding binding, EndpointAddress address)
            : base(binding, address)
        {
            factory = CreateChannel();
        }

        public PolicySetType Load()
        {
            try
            {
                return factory.Load();
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
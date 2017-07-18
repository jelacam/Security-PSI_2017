using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Security;
using Contracts;

namespace Client
{
    public class ClientProxy : ChannelFactory<IContract>, IContract, IDisposable
    {
        IContract factory;

        public ClientProxy(NetTcpBinding binding, EndpointAddress address)
            : base (binding, address)
        {
            factory = this.CreateChannel();
        }
        public string Test()
        {
            try
            {
                return factory.Test();
            }
            catch (Exception e)
            {

                Console.WriteLine("Proxy error while trying to Test. Message: {0}", e.Message);
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

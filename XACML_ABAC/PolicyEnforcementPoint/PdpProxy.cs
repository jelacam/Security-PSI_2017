using Contracts;
using System;
using System.ServiceModel;

namespace PolicyEnforcementPoint
{
    public class PdpProxy : ChannelFactory<IPdpContract>, IPdpContract, IDisposable
    {
        private IPdpContract factory;

        public PdpProxy(NetTcpBinding binding, EndpointAddress address)
            : base(binding, address)
        {
            factory = this.CreateChannel();
        }

        public DecisionType Evaluate()
        {
            try
            {
                return factory.Evaluate();
            }
            catch (Exception e)
            {
                Console.WriteLine("PdpProxy error: Message: {0}", e.Message);
                return DecisionType.Indeterminate;
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
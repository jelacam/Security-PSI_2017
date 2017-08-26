using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Common.Contracts;
using Common;

namespace PolicyEnforcementPoint
{
    public class ContextProxy : ChannelFactory<IContextHandler>, IContextHandler, IDisposable
    {
        IContextHandler factory;

        public ContextProxy(NetTcpBinding binding, EndpointAddress address)
            : base (binding, address)
        {
            factory = this.CreateChannel();
        }

        public DecisionType CheckAccess(Dictionary<string, List<DomainAttribute>> DomainAttributes)
        {
            try
            {
                return factory.CheckAccess(DomainAttributes);
            }
            catch (Exception e)
            {
                Console.WriteLine("Context proxy error: Message: {0}", e.Message);
                return DecisionType.Indeterminate;
            }
        }

        //public List<DomainAttribute> RequestForEnvironmentAttribute(List<string> AttributeTypes)
        //{
        //    try
        //    {
        //        return factory.RequestForEnvironmentAttribute(AttributeTypes);

        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("Context proxy error: Message: {0}", e.Message);
        //        return null;
        //    }
        //}
    }
}

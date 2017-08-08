using Contracts.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class AuthProxy : ChannelFactory<IAuthentication>, IAuthentication, IDisposable
    {
        private IAuthentication factory;

        public AuthProxy(NetTcpBinding binding, EndpointAddress address)
            : base(binding, address)
        {
            factory = CreateChannel();
        }

        public bool IsAuthenticated()
        {
            try
            {
                return factory.IsAuthenticated();
            }
            catch (Exception e)
            {
                Console.WriteLine("Authentication proxy error: Message: {0}", e.Message);
                return false;
            }
        }

        public void SetLocation(string userId, string location)
        {
            try
            {
                factory.SetLocation(userId, location);
            }
            catch (Exception e)
            {
                Console.WriteLine("Authentication proxy error: Message: {0}", e.Message);
            }
        }

        public string AuthenticatedUserId()
        {
            try
            {
                return factory.AuthenticatedUserId();
            }
            catch (Exception e)
            {
                Console.WriteLine("Authentication proxy error: Message: {0}", e.Message);
                return null;
            }
        }

        //public HashSet<string> GetUserInformation(string userId, string attributeId)
        //{
        //    try
        //    {
        //        return factory.GetUserInformation(userId, attributeId);
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("Authentication proxy error: Message: {0}", e.Message);
        //        return null;
        //    }
        //}

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
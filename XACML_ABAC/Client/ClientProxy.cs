using Contracts;
using System;
using System.ServiceModel;

namespace Client
{
    public class ClientProxy : DuplexChannelFactory<IContract>, IContract, IDisposable
    {
        private IContract factory;

        public ClientProxy(NetTcpBinding binding, EndpointAddress address)
            : base(new InstanceContext(new LocationService()), binding, address)
        {
            factory = CreateChannel();
        }

        public string EditRemainingCourses()
        {
            try
            {
                return factory.EditRemainingCourses();
            }
            catch (Exception e)
            {
                Console.WriteLine("Proxy error. Message: {0}", e.Message);
                return null;
            }
        }

        public string ViewRemainingCourses()
        {
            try
            {
                return factory.ViewRemainingCourses();
            }
            catch (Exception e)
            {
                Console.WriteLine("Proxy error. Message: {0}", e.Message);
                return null;
            }
        }

        public string RegisterExam()
        {
            try
            {
                return factory.RegisterExam();
            }
            catch (Exception e)
            {
                Console.WriteLine("Proxy error. Message: {0}", e.Message);
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
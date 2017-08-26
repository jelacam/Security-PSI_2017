using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Common;
using System.Net;
using System.Xml.Linq;
using System.Device.Location;
using System.Text.RegularExpressions;
using System.Threading;

namespace Client
{
    public class Program
    {
        private static void Main(string[] args)
        {
            CLocation myLocation = new CLocation();
            myLocation.GetLocationDataEvent();

            NetTcpBinding binding = new NetTcpBinding();
            binding.CloseTimeout = new TimeSpan(0, 10, 0);
            binding.OpenTimeout = new TimeSpan(0, 10, 0);
            binding.ReceiveTimeout = new TimeSpan(0, 10, 0);
            binding.SendTimeout = new TimeSpan(0, 10, 0);

            string address = "net.tcp://localhost:9999/WcfService";
            string administrationAddress = "net.tcp://localhost:5000/SubjectAdministrationService";

            bool isAuthenticated = false;
            string userId = string.Empty;

            using (AuthProxy authProxy = new AuthProxy(binding, new EndpointAddress(new Uri(administrationAddress))))
            {
                isAuthenticated = authProxy.IsAuthenticated();
                if (isAuthenticated)
                {
                    userId = authProxy.AuthenticatedUserId();
                    //Console.WriteLine("User Location: ");
                    //string location = Console.ReadLine();
                    Console.WriteLine("Location: " + CLocation.location);
                    authProxy.SetLocation(userId, CLocation.location);
                }
            }

            if (isAuthenticated)
            {
                using (ClientProxy proxy = new ClientProxy(binding, new EndpointAddress(new Uri(address))))
                {
                    Console.WriteLine("\nRequest for editing student remaining courses.");
                    string ret = proxy.EditRemainingCourses();
                    Console.WriteLine(ret);
                    // Console.ReadKey();

                    Console.WriteLine("\nRequest for student remaining courses.");
                    ret = proxy.ViewRemainingCourses();
                    Console.WriteLine(ret);
                    //Console.ReadKey();

                    Console.WriteLine("\nRequest for exam registration.");
                    ret = proxy.RegisterExam();
                    Console.WriteLine(ret);
                }
            }
            Console.ReadKey();
        }
    }
}
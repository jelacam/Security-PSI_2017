using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.IdentityModel.Services;
using System.Security.Permissions;
using System.IdentityModel.Claims;

namespace Service
{
    public class WcfService : IContract
    {
        public string AccessDenied()
        {
            Console.WriteLine("Test method for access denied. [Student try to edit remaining courses]");
            return "Edit student remaining courses...";
        }

        public string AccessPermit()
        {
            Console.WriteLine("Test method for access permit. [Student try to view remaining courses]");
            return "Student remaining courses...";
        }
    }
}
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
            return "Edit student remaining courses...";
        }

      
        public string AccessPermit()
        {
         
            return "Student remaining courses...";
        }
    }
}

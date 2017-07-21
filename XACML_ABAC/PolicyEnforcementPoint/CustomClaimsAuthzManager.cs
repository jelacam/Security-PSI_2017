using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PolicyEnforcementPoint
{
    public class CustomClaimsAuthzManager : ClaimsAuthorizationManager
    {
        public override bool CheckAccess(AuthorizationContext context)
        {
           
            return base.CheckAccess(context);
        }
    }
}

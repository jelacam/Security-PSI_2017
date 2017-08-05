using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace PolicyEnforcementPoint.CustomPrincipal
{
    public class AuthzPolicy : IAuthorizationPolicy
    {
        private string id;
        private object locker = new object();

        public ClaimSet Issuer { get => ClaimSet.System; }

        public string Id { get => id; }

        public bool Evaluate(EvaluationContext evaluationContext, ref object state)
        {
            object list;

            if (!evaluationContext.Properties.TryGetValue("Identities", out list))
            {
                return false;
            }

            IList<IIdentity> identities = list as IList<IIdentity>;
            if (list == null || identities.Count <= 0)
            {
                return false;
            }

            evaluationContext.Properties["Principal"] = GetPrincipal(identities[0]);
            return true;
        }

        protected IPrincipal GetPrincipal(IIdentity identity)
        {
            lock (locker)
            {
                WindowsIdentity winIdentity = identity as WindowsIdentity;

                return new CustomPrincipal(winIdentity);
            }
        }
    }
}
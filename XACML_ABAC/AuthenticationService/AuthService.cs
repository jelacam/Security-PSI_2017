using Contracts;
using Contracts.Contracts;
using PolicyEnforcementPoint.CustomPrincipal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService
{
    public class AuthService : IAuthentication, ISubjectInformation
    {
        private static Dictionary<string, string> UserLocation = new Dictionary<string, string>(3);
        private static Dictionary<string, HashSet<string>> UserRole = new Dictionary<string, HashSet<string>>(3);

        private IIdentity identity;
        public IIdentity Identity { get => identity; set => identity = value; }

        private CustomPrincipal customPrincipal;
        public CustomPrincipal CustomPrincipal { get => customPrincipal; set => customPrincipal = value; }

        public bool IsAuthenticated()
        {
            IPrincipal principal = OperationContext.Current.ServiceSecurityContext.AuthorizationContext.Properties["Principal"] as IPrincipal;

            CustomPrincipal = principal as CustomPrincipal;
            Identity = CustomPrincipal.Identity;
            if (Identity.IsAuthenticated)
            {
                SetUserRole();
            }

            return Identity.IsAuthenticated;
        }

        public void SetLocation(string userId, string location)
        {
            UserLocation[userId] = location;
        }

        private void SetUserRole()
        {
            UserRole[AuthenticatedUserId()] = CustomPrincipal.Groups;
        }

        public string GetUserLocation(string userId)
        {
            Console.WriteLine("Request for user: {0} location", userId.ToString());
            string location = null;
            if (UserLocation.TryGetValue(userId, out location))
            {
                return location;
            }

            return null;
        }

        public string AuthenticatedUserId()
        {
            return Identity.Name.Split('\\')[1];
        }

        public HashSet<string> GetUserRoles(string userId)
        {
            Console.WriteLine("Request for user: {0} roles", userId.ToString());
            HashSet<string> userRoles = null;
            if (UserRole.TryGetValue(userId, out userRoles))
            {
                return userRoles;
            }
            return userRoles;
        }

        public HashSet<string> GetUserInformation(string userId, string attributeId)
        {
            HashSet<string> ret = new HashSet<string>();

            switch (attributeId)
            {
                case XacmlSubject.LOCATION:
                    {
                        string location = GetUserLocation(userId);
                        ret.Add(location);
                        break;
                    }
                case XacmlSubject.ROLE:
                    {
                        ret = GetUserRoles(userId);
                        break;
                    }
            }

            return ret;
        }
    }
}
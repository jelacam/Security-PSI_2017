using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace PolicyEnforcementPoint.CustomPrincipal
{
    public class CustomPrincipal : IPrincipal
    {
        private WindowsIdentity winIdentity;

        private HashSet<string> groups = new HashSet<string>();

        public IIdentity Identity { get => winIdentity; }
        public HashSet<string> Groups { get => groups; }

        public CustomPrincipal(WindowsIdentity winIdentity)
        {
            this.winIdentity = winIdentity;

            string groupName;

            foreach (IdentityReference group in this.winIdentity.Groups)
            {
                SecurityIdentifier sid = (SecurityIdentifier)group.Translate(typeof(SecurityIdentifier));
                var name = sid.Translate(typeof(NTAccount));

                if ((name).ToString().Contains('\\'))
                {
                    groupName = name.ToString().Split('\\')[1];
                }
                else
                {
                    groupName = name.ToString();
                }

                Groups.Add(groupName);
            }
        }

        public bool IsInRole(string role)
        {
            return true;
        }
    }
}
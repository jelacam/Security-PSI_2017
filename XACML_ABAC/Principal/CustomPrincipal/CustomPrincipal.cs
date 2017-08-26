using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Principal.CustomPrincipal
{
    public class CustomPrincipal : IPrincipal
    {
        private WindowsIdentity winIdentity;

        private HashSet<string> roles = new HashSet<string>();

        public IIdentity Identity { get => winIdentity; }
        public HashSet<string> Roles { get => roles; }

        public CustomPrincipal(WindowsIdentity winIdentity)
        {
            this.winIdentity = winIdentity;

            string groupName;

            foreach (IdentityReference group in this.winIdentity.Groups)
            {
                try
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

                    Roles.Add(groupName);
                }
                catch (Exception)
                {
                    //Console.WriteLine(e.Message);
                }
            }
        }

        public bool IsInRole(string role)
        {
            return true;
        }
    }
}
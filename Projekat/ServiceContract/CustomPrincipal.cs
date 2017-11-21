using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContract
{
    public class CustomPrincipal : IPrincipal, IDisposable
    {

        private WindowsIdentity identity = null;

        public CustomPrincipal(WindowsIdentity identity)
        {
            this.identity = identity;
        }

        public IIdentity Identity
        {
            get { return this.identity; }
        }

        public bool IsInRole(string role)
        {
            bool IsAuthz = false;

            foreach (IdentityReference group in identity.Groups)
            {
                SecurityIdentifier sid = (SecurityIdentifier)group.Translate(typeof(SecurityIdentifier));
                var name = sid.Translate(typeof(NTAccount));
                if (name.ToString().Contains(role))
                {
                    IsAuthz = true;
                }
            }

            return IsAuthz;
        }

        public void Dispose()
        {
            if (identity != null)
            {
                identity.Dispose();
                identity = null;
            }
        }
    }
}

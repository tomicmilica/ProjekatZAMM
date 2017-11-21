using ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServerPrimary
{
    class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:2800/ServerPrimary";

            ServiceHost host = new ServiceHost(typeof(ServerPrimary));
            host.AddServiceEndpoint(typeof(IService), binding, address);

           
            foreach (IdentityReference group in WindowsIdentity.GetCurrent().Groups)
            {
                SecurityIdentifier sid = (SecurityIdentifier)group.Translate(typeof(SecurityIdentifier));
                var name = sid.Translate(typeof(NTAccount));
                Console.WriteLine(name.Value);
            }

            host.Open();

            Console.WriteLine("SecurityService service is started.");
            Console.WriteLine("Press <enter> to stop service...");




            Console.ReadLine();
        
    }
    }
}

using ServiceContract;
using System;
using System.Collections.Generic;
using System.IdentityModel.Policy;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Description;
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

            host.Authorization.ServiceAuthorizationManager = new AuthorizationManager();
            List<IAuthorizationPolicy> policies = new List<IAuthorizationPolicy>();
            policies.Add(new AuthorizationPolicy());
            host.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.Custom;

            host.Description.Behaviors.Remove(typeof(ServiceDebugBehavior));
            host.Description.Behaviors.Add(new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });

            host.Authorization.ExternalAuthorizationPolicies = policies.AsReadOnly();
            ServiceSecurityAuditBehavior newAudit = new ServiceSecurityAuditBehavior();
            newAudit.AuditLogLocation = AuditLogLocation.Application;
            newAudit.ServiceAuthorizationAuditLevel = AuditLevel.SuccessOrFailure;
            newAudit.SuppressAuditFailure = true;

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

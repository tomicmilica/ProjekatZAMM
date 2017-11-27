using CertificateManager;
using Common;
using ServiceContract;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Policy;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;

namespace Servers
{
    class Program
    {
        static void Main(string[] args)
        {
            //  Console.ReadLine();
            var name = WindowsIdentity.GetCurrent().Name;
            int index = name.IndexOf('\\') + 1;
            string user = name.Substring(index);

            if (user.Contains("Server1") || user.Contains("wcfservice"))
            {
                if (user.Contains("Server1"))
                {
                    ServiceHost host = new ServiceHost(typeof(ServerPrimary));

                    NetTcpBinding binding = new NetTcpBinding();
                    binding.Security.Mode = SecurityMode.Transport;
                    binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;

                    //uzimam iz konfiguracionog fajla adresu klijenta na koju se kaci server
                    ClientSection clientSection = (ClientSection)ConfigurationManager.GetSection("system.serviceModel/client");
                    ChannelEndpointElement endpoint = clientSection.Endpoints[0];
                    string address = string.Format(endpoint.Address.ToString());
                    host.AddServiceEndpoint(typeof(IService), binding, address);

                    //autorizacija na serveru
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

                    //ispisujem grupe bzvz :D
                    foreach (IdentityReference group in WindowsIdentity.GetCurrent().Groups)
                    {
                        SecurityIdentifier sid = (SecurityIdentifier)group.Translate(typeof(SecurityIdentifier));
                        var name1 = sid.Translate(typeof(NTAccount));
                        Console.WriteLine(name1.Value);
                    }

                    host.Open();
                    Console.WriteLine("Primary Service service is started.");
                }
                else
                {
                    ServiceHost hostRepl = new ServiceHost(typeof(ReplicatorServer));
                    hostRepl.Credentials.ServiceCertificate.Certificate = CertificateManagerClass.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, "wcfservice");
                    hostRepl.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.ChainTrust;
                    hostRepl.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

                    NetTcpBinding binding = new NetTcpBinding();
                    binding.Security.Mode = SecurityMode.Transport;
                    binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

                    ClientSection clientSection = (ClientSection)ConfigurationManager.GetSection("system.serviceModel/client");
                    ChannelEndpointElement endpoint = clientSection.Endpoints[1];
                    string address = string.Format(endpoint.Address.ToString());
                    hostRepl.AddServiceEndpoint(typeof(IReplicator), binding, address);

                    Console.WriteLine("Adress of Replicator Server: " + address);

                    hostRepl.Open();
                }
            }
            else if (user.Contains("wcfclient"))
            {
                Console.WriteLine("Prepairing data for replication ...");
                //aktiviram replicator client koji se veze na replicator server

                ReplicatorClient replicator = new ReplicatorClient();
            }
            else
            {
                ServerSecondary second = new ServerSecondary();
                second.ReceivedData();
            }

            Console.ReadLine();
        }
    }
}

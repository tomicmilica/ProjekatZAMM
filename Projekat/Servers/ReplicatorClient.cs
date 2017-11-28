using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;
using System.Threading;
using ServiceContract;
using System.ServiceModel;
using Common;
using System.ServiceModel.Configuration;
using System.ServiceModel.Security;
using CertificateManager;
using DESAlgorithm;

namespace Servers
{
    public class ReplicatorClient : ChannelFactory<IReplicator>, IDisposable
    {

        IReplicator factory;
        Alarm alarm;

        public ReplicatorClient()
        {

            var lastLine = File.ReadLines("../../databaseOfPrimary.txt").Last();
            string[] data = lastLine.Split('|');

            alarm = new Alarm();
            alarm.TimeStamp = Convert.ToDateTime(data[0]);
            alarm.Client = data[1];
            alarm.Message = data[2];
            alarm.Risk = Int16.Parse(data[3]);

           Audit.InitReplication();     //UPIS U EVENT LOG

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            ClientSection clientSection = (ClientSection)ConfigurationManager.GetSection("system.serviceModel/client");
            ChannelEndpointElement endpoint = clientSection.Endpoints[1]; 
            string address = string.Format(endpoint.Address.ToString());
          
            X509Certificate2 srvCert = CertificateManagerClass.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, "wcfservice"); //.cer 
            EndpointAddress endpointAddress = new EndpointAddress(new Uri(address), new X509CertificateEndpointIdentity(srvCert));

            ProcessDES pr = new ProcessDES();
            string forEncrypt = pr.EncryptionStart(alarm.Message, "KljucZaE", false);
    
            //alarm.Message = forEncrypt;

            using (ReplicatorClient proxy = new ReplicatorClient(binding, endpointAddress))
                {
                    proxy.SendToReplicatorServer(alarm);
                }    
        }

        public ReplicatorClient(NetTcpBinding binding, EndpointAddress address) : base(binding, address)
        {
            this.Credentials.ClientCertificate.Certificate = CertificateManagerClass.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, "wcfclient");
            this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.ChainTrust;
            this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            factory = this.CreateChannel();
        }
        
        public void SendToReplicatorServer(Alarm alarm)
        {
            try
            {
                factory.SendToReplicatorServer(alarm);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }
        }
    }
}

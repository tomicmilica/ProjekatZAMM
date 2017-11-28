
using Common;
using DESAlgorithm;
using ServiceContract;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;

namespace Servers
{
    public class ReplicatorServer : IReplicator
    {
        public void SendToReplicatorServer(Alarm alarm)
        {
            Console.WriteLine("SendToReplicatorServer() successfully executed..\n");
           
            try
            {
                ProcessDES pr = new ProcessDES();

                string forEncrypt = pr.DecryptionStart(alarm.Message, "KljucZaE", true);
                alarm.Message = pr.FromBinaryToText(forEncrypt);

                Console.WriteLine("********************************");
                Console.WriteLine("Received from Replicator Client:");
                Console.WriteLine("Time {0}", alarm.TimeStamp);
                Console.WriteLine("Client {0}", alarm.Client);
                Console.WriteLine("Message {0}", alarm.Message);
                Console.WriteLine("Risk {0}", alarm.Risk);

                Audit.ReadReplication();    //upis u EVENT log

                string pathWrite = "../../databaseOfSecondary.txt";
                FileStream fs = new FileStream(pathWrite, FileMode.Append, FileAccess.Write);

                using (StreamWriter outputFile = new StreamWriter(fs))
                {
                    outputFile.WriteLine(alarm.TimeStamp + "|" + alarm.Client + "|" + alarm.Message + "|" + alarm.Risk);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);           
            }
        }
    }
}

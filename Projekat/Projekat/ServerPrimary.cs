using ServiceContract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerPrimary
{
    public class ServerPrimary : IService
    {
        public void SendAlarm(Alarm alarm)
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;

            if (principal.IsInRole("AlarmGenerators"))
            {
                Console.WriteLine("Time: {0}, Client: {1}, Message: {2}, Risk: {3}", alarm.TimeStamp.ToString(), alarm.Client, alarm.Message, alarm.Risk);
                string pathWrite = "../../databaseOfPrimary.txt";
                FileStream fs = new FileStream(pathWrite, FileMode.Append, FileAccess.Write);

                using (StreamWriter outputFile = new StreamWriter(fs))
                {
                    outputFile.WriteLine(alarm.TimeStamp + "|" + alarm.Client + "|" + alarm.Message + "|" + alarm.Risk);
                }

                //ReplicatorClient replicator = new ReplicatorClient();
            }
            else
            {
                Console.WriteLine("You can't send message to server!");
            }

        }
    }
}

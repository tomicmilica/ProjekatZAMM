using Common;
using ServiceContract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Servers
{
    public class ServerPrimary : IService
    {
        public object locker = new object();

        public void SendAlarm(Alarm alarm)
        {
            lock (locker)
            {

                CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;

                try
                {
                    if (principal.IsInRole("AlarmGenerators"))
                    {
                        Console.WriteLine();
                        Console.WriteLine("Received from client:");
                        Console.WriteLine("Time {0}", alarm.TimeStamp);
                        Console.WriteLine("Client {0}", alarm.Client);
                        Console.WriteLine("Message {0}", alarm.Message);
                        Console.WriteLine("Risk {0}", alarm.Risk);

                        string pathWrite = "../../databaseOfPrimary.txt";
                        FileStream fs = new FileStream(pathWrite, FileMode.Append, FileAccess.Write);

                        using (StreamWriter outputFile = new StreamWriter(fs))
                        {
                            outputFile.WriteLine(alarm.TimeStamp + "|" + alarm.Client + "|" + alarm.Message + "|" + alarm.Risk);
                        }
                    }
                    else
                    {
                        Console.WriteLine("You can't send message to server!");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Esception in service: " + e.Message);
                }
            }
        }
    }
}

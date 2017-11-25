using Common;
using ServiceContract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servers
{
    public class ServerSecondary 
    {
        public void ReceivedData()
        {
            var lastLine = File.ReadLines("../../databaseOfSecondary.txt").Last();
            string[] data = lastLine.Split('|');

            Alarm alarm = new Alarm();
            alarm.TimeStamp = Convert.ToDateTime(data[0]);
            alarm.Client = data[1];
            alarm.Message = data[2];
            alarm.Risk = Int16.Parse(data[3]);

            Console.WriteLine("**************************");
            Console.WriteLine("Received from replicator");
            Console.WriteLine("Time {0}", alarm.TimeStamp);
            Console.WriteLine("Client {0}", alarm.Client);
            Console.WriteLine("Message {0}", alarm.Message);
            Console.WriteLine("Risk {0}", alarm.Risk);
        }
    }
}

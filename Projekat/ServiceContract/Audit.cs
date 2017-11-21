using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContract
{
    public class Audit
    {
        private static EventLog customLog = null;
        const string SourceName = "Common.Audit";
        const string LogName = "ReplicatorLog";

        public Audit()
        {
            try
            {
                if (!EventLog.SourceExists(SourceName))
                {
                    EventLog.CreateEventSource(SourceName, LogName);
                }

                customLog = new EventLog(LogName, Environment.MachineName, SourceName);
            }
            catch (Exception e)
            {
                customLog = null;
                Console.WriteLine("Error while trying to create log handle. Error: {0}", e.Message);
            }
        }

        public static string Mssg1(string username)
        {
            string message = String.Format(AuditEvents.Mssg1, username);
            return message;
        }

        public static string Mssg2()
        {
            string message = String.Format(AuditEvents.Mssg2);
            return message;
        }

        public static string Mssg3()
        {
            string message = String.Format(AuditEvents.Mssg3);
            return message;
        }

        public static string InitReplication()
        {
            string message = String.Format(AuditEvents.InitReplication);
            return message;
        }

        public static string ReadReplication()
        {
            string message = String.Format(AuditEvents.ReadReplication);
            return message;
        }
    }
}

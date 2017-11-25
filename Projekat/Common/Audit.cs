using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Audit
    {
        private static EventLog customLog = null;
        const string SourceName = "Common.Audit";
        const string LogName = "LogName";

        static Audit()
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

        public static void InitReplication()
        {
            string message = String.Format(AuditEvents.InitReplication);
            if (customLog != null)
            {
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event to event log."));
            }
        }

        public static void ReadReplication()
        {
            string message = String.Format(AuditEvents.ReadReplication);
            if (customLog != null)
            {
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event to event log."));
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContract
{
    public enum AuditEventTypes
    {
        Mssg1 = 0,
        Mssg2 = 1,
        Mssg3 = 2,
        InitReplication = 3,
        ReadReplication = 4
    }

    public class AuditEvents
    {
        private static ResourceManager resourceManager = null;
        private static object resourceLock = new object();

        private static ResourceManager ResourceMgr
        {
            get
            {
                lock (resourceLock)
                {
                    if (resourceManager == null)
                    {
                        resourceManager = new ResourceManager(typeof(Messages).FullName, Assembly.GetExecutingAssembly());
                    }
                    return resourceManager;
                }
            }
        }

        public static string Mssg1
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.Mssg1.ToString());
            }
        }

        public static string Mssg2
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.Mssg2.ToString());
            }
        }

        public static string Mssg3
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.Mssg3.ToString());
            }
        }

        public static string InitReplication
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.InitReplication.ToString());
            }
        }

        public static string ReadReplication
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.ReadReplication.ToString());
            }
        }
    }
}

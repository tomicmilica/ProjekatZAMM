using ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;

            string address = "net.tcp://localhost:2800/ServerPrimary";

            using (ClientProxy proxy = new ClientProxy(binding, address))
            {
                string Client = WindowsIdentity.GetCurrent().Name.ToString();

                Console.WriteLine("Username: " + WindowsIdentity.GetCurrent().Name);
                Console.WriteLine("Group: ");
                bool inGroup = false;

                foreach (IdentityReference group in WindowsIdentity.GetCurrent().Groups)
                {
                    SecurityIdentifier sid = (SecurityIdentifier)group.Translate(typeof(SecurityIdentifier));
                    var name = sid.Translate(typeof(NTAccount));
                    if (name.ToString().Contains("AlarmGenerators"))
                    {
                        inGroup = true;
                    }
                    if (inGroup)
                        Console.WriteLine(name.Value);
                }

                Alarm a = new Alarm();
                a.Client = WindowsIdentity.GetCurrent().Name;
                a.Risk = a.CalculateRisk();
                a.TimeStamp = DateTime.Now;

                if (!inGroup)
                {
                    a.Message = Audit.Mssg1(Client);
                }
                else
                {
                    a.Message = Audit.Mssg2();
                }

                proxy.SendAlarm(a);
            }

            Console.ReadLine();

        }
    }
}

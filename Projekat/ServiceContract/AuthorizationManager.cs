using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContract
{
    public class AuthorizationManager : ServiceAuthorizationManager
    {

        //vraca da li moze da se pozove neka metoda
        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            bool isAuth = false;
            //skinemo principal i procerimo permisisje
            IPrincipal principal = operationContext.ServiceSecurityContext.AuthorizationContext.Properties["Principal"] as IPrincipal;
            //svaka grupa ima svoje permisije- operacije koje moze da izvrsava korisnik pripada grupama ili grupi i u zavisnosti od toga moze da radi

            if (principal != null)
            {
                if (principal.IsInRole("AlarmGenerators"))
                {
                    isAuth = true;
                }
            }

            return isAuth;
        }
    }
}

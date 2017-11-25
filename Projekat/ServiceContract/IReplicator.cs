
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContract
{
    [ServiceContract]
    public interface IReplicator
    {
        [OperationContract]
        void SendToReplicatorServer(Alarm alarm);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContract
{
    [DataContract]
    public class Alarm
    {

        private DateTime timeStamp;
        private string client;
        private string message;
        private int risk;

        [DataMember]
        public DateTime TimeStamp
        {
            get
            {
                return timeStamp;
            }

            set
            {
                timeStamp = value;
            }
        }

        [DataMember]
        public string Client
        {
            get
            {
                return client;
            }

            set
            {
                client = value;
            }
        }

        [DataMember]
        public string Message
        {
            get
            {
                return message;
            }

            set
            {
                message = value;
            }
        }

        [DataMember]
        public int Risk
        {
            get
            {
                return risk;
            }

            set
            {
                risk = value;
            }
        }

        public int CalculateRisk()
        {
            int random = 0;

            Random rand = new Random();
            random = rand.Next(1, 10);

            return random;
        }
    }
}

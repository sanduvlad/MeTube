using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageQueue.MessagingProtocol
{
    public class CommunicationType<T>
    {
        public T MessageData { get; set; }
        public TransportType TransportType { get; set; }
    }

    public enum TransportType
    {
        get,
        post,
        put,
        delete
    }
}

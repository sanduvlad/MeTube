using MessageQueue.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageQueue.Storage
{
    public interface IQueueManager
    {
        public Guid AddMessage(Message message);
        public bool RemoveMessage(Guid messageId);
        public Message GetMessage(Guid messageId);
    }
}

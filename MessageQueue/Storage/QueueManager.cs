using MessageQueue.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageQueue.Storage
{
    public class QueueManager : IQueueManager
    {
        private Dictionary<Guid, Message> Messages = new Dictionary<Guid, Message>();

        public Guid AddMessage(Message message)
        {
            var messageId = Guid.NewGuid();
            Messages.Add(messageId, message);

            return messageId;
        }

        public Message PopMessage(Guid messageId)
        {
            Message result = GetMessage(messageId);
            RemoveMessage(messageId);

            return result;
        }

        public Message GetMessage(Guid messageId)
        {
            Message result = null;

            if (Messages.TryGetValue(messageId, out var message))
            {
                result = message;
            }

            return result;
        }

        public bool RemoveMessage(Guid messageId)
        {
            var result = false;
            if (Messages.TryGetValue(messageId, out var message))
            {
                Messages.Remove(messageId);
                result = true;
            }

            return result;
        }
    }
}

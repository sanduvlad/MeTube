using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageQueue.Models
{
    public class Message
    {
        public string Name { get; set; }
        public string Data { get; set; } // dev - why string ?
    }
}

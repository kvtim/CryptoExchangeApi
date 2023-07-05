using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Messages.Events
{
    public class CreateNewLogEvent : IntegrationBaseEvent
    {
        public string? Microservice { get; set; }
        public string? LogType { get; set; }
        public string? Message { get; set; }
        public DateTime LogTime { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger.Models
{
    public class Log
    {
        public string? Microservice { get; set; }
        public string? LogType { get; set; }
        public string? Message { get; set; }
        public DateTime? LogTime { get; set; }
    }
}

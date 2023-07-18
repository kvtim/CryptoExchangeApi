using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Core.KafkaService
{
    public interface IKafkaProducerService
    {
        Task<bool> SendMessage(string topic, string message);
    }
}

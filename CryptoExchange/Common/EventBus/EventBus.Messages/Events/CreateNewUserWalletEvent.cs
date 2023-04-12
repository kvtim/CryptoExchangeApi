using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Messages.Events
{
    public class CreateNewUserWalletEvent : IntegrationBaseEvent
    {
        public required int UserId { get; set; }
    }
}

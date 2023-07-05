using EventBus.Messages.Events;
using Logger.Logger;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger.EventBusConsumer
{
    public class CreateNewLogConsumer : IConsumer<CreateNewLogEvent>
    {
        IElasticLogger _logger;

        public CreateNewLogConsumer(IElasticLogger logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<CreateNewLogEvent> context)
        {
            await _logger.AddOrUpdateLog(
                context.Message.Microservice,
                context.Message.LogType,
                context.Message.Message,
                context.Message.LogTime);

        }
    }
}

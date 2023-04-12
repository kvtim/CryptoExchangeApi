using EventBus.Messages.Events;
using FinanceManagement.Core.Dtos.Wallet;
using FinanceManagement.Core.ErrorHandling;
using FinanceManagement.Core.Models;
using FinanceManagement.Data.Wallets.Commands.CreateWallet;
using MassTransit;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Api.EventBusConsumer
{
    public class CreateNewUserWalletConsumer : IConsumer<CreateNewUserWalletEvent>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CreateNewUserWalletConsumer> _logger;

        public CreateNewUserWalletConsumer(
            IMediator mediator,
            ILogger<CreateNewUserWalletConsumer> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<CreateNewUserWalletEvent> context)
        {
            var command = new CreateWalletCommand()
            {
                Wallet = new Wallet()
                {
                    UserId = context.Message.UserId,
                    CurrencyId = 1,
                    CurrencyAmount = 0
                }
            };

            var result =  await _mediator.Send(command);

            _logger.LogInformation($"CreateNewUserWalletEvent consumed successful!" +
                $"Created wallet with id: {result.ValueOrDefault.Id}");
        }
    }
}

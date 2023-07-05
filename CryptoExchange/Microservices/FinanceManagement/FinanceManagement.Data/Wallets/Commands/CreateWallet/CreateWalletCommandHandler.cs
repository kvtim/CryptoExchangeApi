using AutoMapper;
using EventBus.Messages.Events;
using FinanceManagement.Core.Dtos.Wallet;
using FinanceManagement.Core.ErrorHandling;
using FinanceManagement.Core.Models;
using FinanceManagement.Core.Repositories;
using MassTransit;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Data.Wallets.Commands.CreateWallet
{
    public class CreateWalletCommandHandler : IRequestHandler<CreateWalletCommand, Result<WalletDto>>
    {
        private readonly IWalletRepository _repository;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;

        public CreateWalletCommandHandler(
            IWalletRepository repository,
            IMapper mapper,
            IPublishEndpoint publishEndpoint)
        {
            _repository = repository;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<Result<WalletDto>> Handle(CreateWalletCommand request,
            CancellationToken cancellationToken)
        {
            var wallet = await _repository.GetWalletByUserAndCurrencyAsync(
                request.Wallet.UserId,
                request.Wallet.CurrencyId);

            if (wallet != null)
            {
                await _publishEndpoint.Publish(new CreateNewLogEvent()
                {
                    Microservice = "Finance",
                    LogType = "Exception",
                    Message = $"Wallet {request.Wallet.Id}. " +
                    $"of user {request.Wallet.UserId} already exists",
                    LogTime = DateTime.Now
                });

                return Result.Failure(ErrorType.BadRequest, "Wallet already exists");
            }

            await _repository.AddAsync(request.Wallet);
            await _repository.SaveChangesAsync();

            await _publishEndpoint.Publish(new CreateNewLogEvent()
            {
                Microservice = "Finance",
                LogType = "Addition",
                Message = $"Wallet {request.Wallet.Id}. " +
                $"of user {request.Wallet.UserId} created",
                LogTime = DateTime.Now
            });

            return Result.Ok(_mapper.Map<WalletDto>(request.Wallet));
        }
    }
}
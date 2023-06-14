using AutoMapper;
using FinanceManagement.Core.Dtos.Wallet;
using FinanceManagement.Core.ErrorHandling;
using FinanceManagement.Core.Models;
using FinanceManagement.Core.Repositories;
using FinanceManagement.Data.Logger;
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
        private readonly IFinanceLogger _logger;

        public CreateWalletCommandHandler(IWalletRepository repository, IMapper mapper,
            IFinanceLogger financeLogger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = financeLogger;
        }

        public async Task<Result<WalletDto>> Handle(CreateWalletCommand request,
            CancellationToken cancellationToken)
        {
            var wallet = await _repository.GetWalletByUserAndCurrencyAsync(
                request.Wallet.UserId,
                request.Wallet.CurrencyId);

            if (wallet != null)
            {
                await _logger.AddOrUpdateLog(
                    LogType.Exception,
                    $"Wallet {request.Wallet.Id}. " +
                    $"of user {request.Wallet.UserId} already exists",
                    DateTime.Now);
                return Result.Failure(ErrorType.BadRequest, "Wallet already exists");
            }

            await _repository.AddAsync(request.Wallet);
            await _repository.SaveChangesAsync();

            await _logger.AddOrUpdateLog(
                LogType.Addition,
                $"Wallet {request.Wallet.Id}. " +
                $"of user {request.Wallet.UserId} created",
                DateTime.Now);
            return Result.Ok(_mapper.Map<WalletDto>(request.Wallet));
        }
    }
}

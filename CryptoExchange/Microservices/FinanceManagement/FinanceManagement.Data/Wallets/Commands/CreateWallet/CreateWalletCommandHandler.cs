using AutoMapper;
using FinanceManagement.Core.Dtos.Wallet;
using FinanceManagement.Core.ErrorHandling;
using FinanceManagement.Core.Models;
using FinanceManagement.Core.Repositories;
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

        public CreateWalletCommandHandler(IWalletRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<WalletDto>> Handle(CreateWalletCommand request,
            CancellationToken cancellationToken)
        {
            var wallet = await _repository.GetWalletByUserAndCurrencyAsync(
                request.Wallet.UserId,
                request.Wallet.CurrencyId);

            if (wallet != null)
            {
                return Result.Failure(ErrorType.BadRequest, "Wallet already exists");
            }

            await _repository.AddAsync(request.Wallet);
            await _repository.SaveChangesAsync();

            return Result.Ok(_mapper.Map<WalletDto>(request.Wallet));
        }
    }
}

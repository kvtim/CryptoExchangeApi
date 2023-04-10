﻿using AutoMapper;
using FinanceManagement.Core.Dtos.Wallet;
using FinanceManagement.Core.ErrorHandling;
using FinanceManagement.Core.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Data.Wallets.Commands.UpdateWallet
{
    public class UpdateWalletCommandHandler : IRequestHandler<UpdateWalletCommand, Result<WalletDto>>
    {
        private readonly IWalletRepository _repository;
        private readonly IMapper _mapper;

        public UpdateWalletCommandHandler(IWalletRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<WalletDto>> Handle(UpdateWalletCommand request, CancellationToken cancellationToken)
        {
            var wallet = await _repository.GetByIdAsync(request.Id);

            if (wallet == null)
            {
                return Result.Failure(ErrorType.NotFound, "Wallet not found");
            }

            wallet.CurrencyAmount = request.CurrencyAmount;

            await _repository.UpdateAsync(wallet);
            await _repository.SaveChangesAsync();

            return Result.Ok(_mapper.Map<WalletDto>(wallet));
        }
    }
}

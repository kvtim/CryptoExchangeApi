using AutoMapper;
using FinanceManagement.Core.Dtos.Wallet;
using FinanceManagement.Core.ErrorHandling;
using FinanceManagement.Core.Logger;
using FinanceManagement.Core.Models;
using FinanceManagement.Core.Repositories;
using FinanceManagement.Data.Logger;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Data.Wallets.Queries.GetWalletById
{
    public class GetWalletByIdQueryHandler :
        IRequestHandler<GetWalletByIdQuery, Result<WalletDto>>
    {
        private readonly IWalletRepository _repository;
        private readonly IMapper _mapper;
        private readonly IFinanceLogger _logger;

        public GetWalletByIdQueryHandler(IWalletRepository repository, IMapper mapper,
            IFinanceLogger financeLogger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = financeLogger;
        }

        public async Task<Result<WalletDto>> Handle(GetWalletByIdQuery request, CancellationToken cancellationToken)
        {
            var wallet = await _repository.GetByIdAsync(request.Id);

            if (wallet == null)
            {
                await _logger.AddOrUpdateLog(
                    LogType.Exception,
                    $"Wallet {request.Id} not found ",
                    DateTime.Now);

                return Result.Failure(ErrorType.NotFound, "Wallet not found");
            }

            return Result.Ok(_mapper.Map<WalletDto>(wallet));
        }
    }
}
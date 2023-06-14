using AutoMapper;
using FinanceManagement.Core.Dtos.Wallet;
using FinanceManagement.Core.ErrorHandling;
using FinanceManagement.Core.Models;
using FinanceManagement.Core.Repositories;
using FinanceManagement.Data.Logger;
using FinanceManagement.Data.Wallets.Queries.GetWalletList;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Data.Wallets.Queries.GetAllWallets
{
    public class GetAllWalletsQueryHandler :
        IRequestHandler<GetAllWalletsQuery, Result<IEnumerable<WalletDto>>>
    {
        private readonly IWalletRepository _repository;
        private readonly IMapper _mapper;
        private readonly IFinanceLogger _logger;

        public GetAllWalletsQueryHandler(IWalletRepository repository, IMapper mapper,
            IFinanceLogger financeLogger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = financeLogger;
        }

        public async Task<Result<IEnumerable<WalletDto>>> Handle(GetAllWalletsQuery request, CancellationToken cancellationToken)
        {
            var wallets = await _repository.GetAllAsync();

            if (!wallets.Any())
            {
                await _logger.AddOrUpdateLog(
                    LogType.Exception,
                    $"Wallets not found ",
                    DateTime.Now);

                return Result.Failure(ErrorType.NotFound, "Wallets not found");
            }

            return Result.Ok(_mapper.Map<IEnumerable<WalletDto>>(wallets));
        }
    }
}

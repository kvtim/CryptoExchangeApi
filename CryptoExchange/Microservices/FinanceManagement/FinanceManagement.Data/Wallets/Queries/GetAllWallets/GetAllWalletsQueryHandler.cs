using AutoMapper;
using FinanceManagement.Core.Dtos.Wallet;
using FinanceManagement.Core.Repositories;
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
        IRequestHandler<GetAllWalletsQuery, IEnumerable<WalletDto>>
    {
        private readonly IWalletRepository _repository;
        private readonly IMapper _mapper;

        public GetAllWalletsQueryHandler(IWalletRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<WalletDto>> Handle(GetAllWalletsQuery request, CancellationToken cancellationToken)
        {
            var wallets = await _repository.GetAllAsync();

            return _mapper.Map<IEnumerable<WalletDto>>(wallets);
        }
    }
}

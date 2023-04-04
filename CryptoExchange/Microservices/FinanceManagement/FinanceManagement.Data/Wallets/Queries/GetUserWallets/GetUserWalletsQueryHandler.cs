using AutoMapper;
using FinanceManagement.Core.Dtos.Wallet;
using FinanceManagement.Core.Repositories;
using FinanceManagement.Data.Wallets.Queries.GetWalletList;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Data.Wallets.Queries.GetUserWallets
{
    public class GetUserWalletsQueryHandler :
        IRequestHandler<GetUserWalletsQuery, IEnumerable<WalletDto>>
    {
        private readonly IWalletRepository _repository;
        private readonly IMapper _mapper;

        public GetUserWalletsQueryHandler(IWalletRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<WalletDto>> Handle(GetUserWalletsQuery request, CancellationToken cancellationToken)
        {
            var wallets = await _repository.GetUserWalletsAsync(request.UserId);

            return _mapper.Map<IEnumerable<WalletDto>>(wallets);
        }
    }
}

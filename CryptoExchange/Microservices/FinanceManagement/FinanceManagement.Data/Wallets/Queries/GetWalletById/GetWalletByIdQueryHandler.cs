using AutoMapper;
using FinanceManagement.Core.Dtos.Wallet;
using FinanceManagement.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Data.Wallets.Queries.GetWalletById
{
    public class GetWalletByIdQueryHandler :
        IRequestHandler<GetWalletByIdQuery, WalletDto>
    {
        private readonly IWalletRepository _repository;
        private readonly IMapper _mapper;

        public GetWalletByIdQueryHandler(IWalletRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<WalletDto> Handle(GetWalletByIdQuery request, CancellationToken cancellationToken)
        {
            var wallet = await _repository.GetByIdAsync(request.Id);

            return _mapper.Map<WalletDto>(wallet);
        }
    }
}

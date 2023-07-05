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

namespace FinanceManagement.Data.Wallets.Queries.GetWalletById
{
    public class GetWalletByIdQueryHandler :
        IRequestHandler<GetWalletByIdQuery, Result<WalletDto>>
    {
        private readonly IWalletRepository _repository;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;

        public GetWalletByIdQueryHandler(IWalletRepository repository, IMapper mapper,
            IPublishEndpoint publishEndpoint)
        {
            _repository = repository;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<Result<WalletDto>> Handle(GetWalletByIdQuery request, CancellationToken cancellationToken)
        {
            var wallet = await _repository.GetByIdAsync(request.Id);

            if (wallet == null)
            {
                await _publishEndpoint.Publish(new CreateNewLogEvent()
                {
                    Microservice = "Finance",
                    LogType = "Exception",
                    Message = $"Wallet {request.Id} not found ",
                    LogTime = DateTime.Now
                });

                return Result.Failure(ErrorType.NotFound, "Wallet not found");
            }

            return Result.Ok(_mapper.Map<WalletDto>(wallet));
        }
    }
}
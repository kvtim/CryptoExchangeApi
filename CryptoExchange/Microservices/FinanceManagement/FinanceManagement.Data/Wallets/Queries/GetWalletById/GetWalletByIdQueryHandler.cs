using AutoMapper;
using EventBus.Messages.Common;
using EventBus.Messages.Events;
using FinanceManagement.Core.Dtos.Wallet;
using FinanceManagement.Core.ErrorHandling;
using FinanceManagement.Core.KafkaService;
using FinanceManagement.Core.Models;
using FinanceManagement.Core.Repositories;
using MassTransit;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FinanceManagement.Data.Wallets.Queries.GetWalletById
{
    public class GetWalletByIdQueryHandler :
        IRequestHandler<GetWalletByIdQuery, Result<WalletDto>>
    {
        private readonly IWalletRepository _repository;
        private readonly IMapper _mapper;
        private readonly IKafkaProducerService _kafkaProducerService;

        public GetWalletByIdQueryHandler(
            IWalletRepository repository,
            IMapper mapper,
            IKafkaProducerService kafkaProducerService)
        {
            _repository = repository;
            _mapper = mapper;
            _kafkaProducerService = kafkaProducerService;
        }

        public async Task<Result<WalletDto>> Handle(GetWalletByIdQuery request, CancellationToken cancellationToken)
        {
            var wallet = await _repository.GetByIdAsync(request.Id);

            if (wallet == null)
            {
                await _kafkaProducerService.SendMessage(
                    TopicNamesConstants.FinanceLogsTopic,
                    JsonSerializer.Serialize(new CreateNewLogEvent()
                    {
                        Microservice = "Finance",
                        LogType = "Exception",
                        Message = $"Wallet {request.Id} not found ",
                        LogTime = DateTime.Now
                    }));

                return Result.Failure(ErrorType.NotFound, "Wallet not found");
            }

            return Result.Ok(_mapper.Map<WalletDto>(wallet));
        }
    }
}
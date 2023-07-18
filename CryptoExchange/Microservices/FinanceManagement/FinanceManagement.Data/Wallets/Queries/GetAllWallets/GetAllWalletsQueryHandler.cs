using AutoMapper;
using EventBus.Messages.Common;
using EventBus.Messages.Events;
using FinanceManagement.Core.Dtos.Wallet;
using FinanceManagement.Core.ErrorHandling;
using FinanceManagement.Core.KafkaService;
using FinanceManagement.Core.Models;
using FinanceManagement.Core.Repositories;
using FinanceManagement.Data.Wallets.Queries.GetWalletList;
using MassTransit;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FinanceManagement.Data.Wallets.Queries.GetAllWallets
{
    public class GetAllWalletsQueryHandler :
        IRequestHandler<GetAllWalletsQuery, Result<IEnumerable<WalletDto>>>
    {
        private readonly IWalletRepository _repository;
        private readonly IMapper _mapper;
        private readonly IKafkaProducerService _kafkaProducerService;

        public GetAllWalletsQueryHandler(
            IWalletRepository repository, 
            IMapper mapper,
            IKafkaProducerService kafkaProducerService)
        {
            _repository = repository;
            _mapper = mapper;
            _kafkaProducerService = kafkaProducerService;
        }

        public async Task<Result<IEnumerable<WalletDto>>> Handle(GetAllWalletsQuery request, CancellationToken cancellationToken)
        {
            var wallets = await _repository.GetAllAsync();

            if (!wallets.Any())
            {
                await _kafkaProducerService.SendMessage(
                    TopicNamesConstants.FinanceLogsTopic,
                    JsonSerializer.Serialize(new CreateNewLogEvent()
                    {
                        Microservice = "Finance",
                        LogType = "Exception",
                        Message = "Wallets not found.",
                        LogTime = DateTime.Now
                    }));

                return Result.Failure(ErrorType.NotFound, "Wallets not found");
            }

            return Result.Ok(_mapper.Map<IEnumerable<WalletDto>>(wallets));
        }
    }
}
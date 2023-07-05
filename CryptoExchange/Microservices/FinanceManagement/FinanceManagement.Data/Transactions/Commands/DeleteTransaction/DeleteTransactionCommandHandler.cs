using EventBus.Messages.Events;
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

namespace FinanceManagement.Data.Transactions.Commands.DeleteTransaction
{
    public class DeleteTransactionCommandHandler : IRequestHandler<DeleteTransactionCommand, Result>
    {
        private readonly ITransactionRepository _repository;
        private readonly IPublishEndpoint _publishEndpoint;

        public DeleteTransactionCommandHandler(ITransactionRepository repository,
            IPublishEndpoint publishEndpoint)
        {
            _repository = repository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<Result> Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
        {
            var transaction = await _repository.GetByIdAsync(request.Id);

            if (transaction == null)
            {
                await _publishEndpoint.Publish(new CreateNewLogEvent()
                {
                    Microservice = "Finance",
                    LogType = "Exception",
                    Message = $"Transaction {request.Id} not found",
                    LogTime = DateTime.Now
                });

                return Result.Failure(ErrorType.NotFound, "Transaction not found");
            }

            await _repository.RemoveAsync(transaction);
            await _repository.SaveChangesAsync();

            await _publishEndpoint.Publish(new CreateNewLogEvent()
            {
                Microservice = "Finance",
                LogType = "Deletion",
                Message = $"Transaction {request.Id} deleted",
                LogTime = DateTime.Now
            });

            return Result.Ok();
        }
    }
}
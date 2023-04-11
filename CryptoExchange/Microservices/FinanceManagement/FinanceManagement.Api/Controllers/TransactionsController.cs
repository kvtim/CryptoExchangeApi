using AutoMapper;
using FinanceManagement.Core.Dtos.Transaction;
using FinanceManagement.Core.Dtos.Wallet;
using FinanceManagement.Core.ErrorHandling;
using FinanceManagement.Core.Models;
using FinanceManagement.Data.Transactions.Commands.CreateTransaction;
using FinanceManagement.Data.Transactions.Commands.DeleteTransaction;
using FinanceManagement.Data.Transactions.Commands.UpdateTransaction;
using FinanceManagement.Data.Transactions.Queries.GetAllTransactions;
using FinanceManagement.Data.Transactions.Queries.GetTransactionById;
using FinanceManagement.Data.Transactions.Queries.GetUserTransactions;
using FinanceManagement.Data.Wallets.Commands.CreateWallet;
using FinanceManagement.Data.Wallets.Commands.DeleteWallet;
using FinanceManagement.Data.Wallets.Commands.UpdateWallet;
using FinanceManagement.Data.Wallets.Queries.GetAllWallets;
using FinanceManagement.Data.Wallets.Queries.GetWalletById;
using FinanceManagement.Data.Wallets.Queries.GetWalletList;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FinanceManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public TransactionsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ApiResult<IEnumerable<TransactionDto>>> GetAllTransactions()
        {
            var transactionsResult = await _mediator.Send(new GetAllTransactionsQuery());

            if (!transactionsResult.Succeeded)
            {
                return ApiResult.Failure(transactionsResult.Error);
            }

            return ApiResult.Ok(transactionsResult);
        }

        [HttpGet("{id}")]
        public async Task<ApiResult<TransactionDto>> GetTransactionById(int id)
        {
            var transactionResult = await _mediator.Send(new GetTransactionByIdQuery() { Id = id });

            if (!transactionResult.Succeeded)
            {
                return ApiResult.Failure(transactionResult.Error);
            }

            return ApiResult.Ok(transactionResult);
        }

        [HttpGet("GetUserTransactions/{userId}")]
        public async Task<ApiResult<IEnumerable<TransactionDto>>> GetUserTransactions(int userId)
        {
            var transactionsResult = await _mediator.Send(new GetUserTransactionsQuery() { UserId = userId });

            if (!transactionsResult.Succeeded)
            {
                return ApiResult.Failure(transactionsResult.Error);
            }
            return ApiResult.Ok(transactionsResult);
        }

        [HttpPost]
        public async Task<ApiResult<TransactionDto>> Add(
            [FromBody] CreateTransactionDto createTransactionDto)
        {
            var transactionResult = await _mediator.Send(new CreateTransactionCommand()
            {
                Transaction = _mapper.Map<Transaction>(createTransactionDto)
            });

            if (!transactionResult.Succeeded)
            {
                return ApiResult.Failure(transactionResult.Error);
            }

            return ApiResult.Ok(transactionResult);
        }

        [HttpPut("{id}")]
        public async Task<ApiResult<TransactionDto>> Update(int id,
            [FromBody] UpdateTransactionDto updateTransactionDto)
        {
            var transactionResult = await _mediator.Send(new UpdateTransactionCommand()
            {
                Id = id,
                NewCurrencyAmount = updateTransactionDto.NewCurrencyAmount,
                FullTransactionPriceUSD = updateTransactionDto.FullTransactionPriceUSD,
            });

            if (!transactionResult.Succeeded)
            {
                return ApiResult.Failure(transactionResult.Error);
            }

            return ApiResult.Ok(transactionResult);
        }

        [HttpDelete("{id}")]
        public async Task<ApiResult> Delete(int id)
        {
            var transactionResult = await _mediator.Send(new DeleteTransactionCommand() { Id = id });

            if (!transactionResult.Succeeded)
            {
                return ApiResult.Failure(transactionResult.Error);
            }
            return ApiResult.Ok();
        }
    }
}

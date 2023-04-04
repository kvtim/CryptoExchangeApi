using AutoMapper;
using FinanceManagement.Core.Dtos.Transaction;
using FinanceManagement.Core.Dtos.Wallet;
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
        public async Task<IActionResult> GetAllTransactions()
        {
            var transactions = await _mediator.Send(new GetAllTransactionsQuery());

            return Ok(transactions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransactionById(int id)
        {
            var transactions = await _mediator.Send(new GetTransactionByIdQuery() { Id = id });

            return Ok(transactions);
        }

        [HttpGet("GetUserTransactions/{userId}")]
        public async Task<IActionResult> GetUserTransactions(int userId)
        {
            var transactions = await _mediator.Send(new GetUserTransactionsQuery() { UserId = userId });

            return Ok(transactions);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateTransactionDto createTransactionDto)
        {
            var transaction = await _mediator.Send(new CreateTransactionCommand()
            {
                Transaction = _mapper.Map<Transaction>(createTransactionDto)
            });

            return Ok(transaction);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,
            [FromBody] UpdateTransactionDto updateTransactionDto)
        {
            var transaction = await _mediator.Send(new UpdateTransactionCommand()
            {
                Id = id,
                NewCurrencyAmount = updateTransactionDto.NewCurrencyAmount,
                FullTransactionPriceUSD = updateTransactionDto.FullTransactionPriceUSD,
            });

            return Ok(transaction);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteTransactionCommand() { Id = id });
            return Ok();
        }
    }
}

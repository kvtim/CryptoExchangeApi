using AutoMapper;
using FinanceManagement.Core.Dtos.Wallet;
using FinanceManagement.Core.Models;
using FinanceManagement.Data.Wallets.Commands.CreateWallet;
using FinanceManagement.Data.Wallets.Commands.DeleteWallet;
using FinanceManagement.Data.Wallets.Commands.UpdateWallet;
using FinanceManagement.Data.Wallets.Queries.GetAllWallets;
using FinanceManagement.Data.Wallets.Queries.GetWalletById;
using FinanceManagement.Data.Wallets.Queries.GetWalletList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public WalletsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWallets()
        {
            var wallets = await _mediator.Send(new GetAllWalletsQuery());

            return Ok(wallets);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWalletById(int id)
        {
            var wallets = await _mediator.Send(new GetWalletByIdQuery() { Id = id });

            return Ok(wallets);
        }

        [HttpGet("GetUserCurrencies/{userId}")]
        public async Task<IActionResult> GetUserWallets(int userId)
        {
            var wallets = await _mediator.Send(new GetUserWalletsQuery() { UserId = userId });

            return Ok(wallets);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateWalletDto createWalletDto)
        {
            var wallet = await _mediator.Send(new CreateWalletCommand()
            {
                Wallet = _mapper.Map<Wallet>(createWalletDto)
            });

            return Ok(wallet);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateWalletDto updateWalletDto)
        {
            var wallet = await _mediator.Send(new UpdateWalletCommand()
            {
                Id = id,
                CurrencyAmount = updateWalletDto.CurrencyAmount
            });

            return Ok(wallet);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteWalletCommand() { Id = id });
            return Ok();
        }
    }
}

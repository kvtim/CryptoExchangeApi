using AutoMapper;
using FinanceManagement.Core.Dtos.Wallet;
using FinanceManagement.Core.ErrorHandling;
using FinanceManagement.Core.Models;
using FinanceManagement.Data.Wallets.Commands.CreateWallet;
using FinanceManagement.Data.Wallets.Commands.DeleteWallet;
using FinanceManagement.Data.Wallets.Commands.UpdateWallet;
using FinanceManagement.Data.Wallets.Queries.GetAllWallets;
using FinanceManagement.Data.Wallets.Queries.GetWalletById;
using FinanceManagement.Data.Wallets.Queries.GetWalletList;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<IEnumerable<WalletDto>>> GetAllWallets()
        {
            var walletsResult = await _mediator.Send(new GetAllWalletsQuery());

            if (!walletsResult.Succeeded)
            {
                return ApiResult.Failure(walletsResult.Error);
            }

            return ApiResult.Ok(walletsResult);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<WalletDto>> GetWalletById(int id)
        {
            var walletResult = await _mediator.Send(new GetWalletByIdQuery() { Id = id });

            if (!walletResult.Succeeded)
            {
                return ApiResult.Failure(walletResult.Error);
            }

            return ApiResult.Ok(walletResult);
        }

        [HttpGet("get-user-currencies/{userId}")]
        public async Task<ApiResult<IEnumerable<WalletDto>>> GetUserWallets(int userId)
        {
            var walletsResult = await _mediator.Send(new GetUserWalletsQuery() { UserId = userId });

            if (!walletsResult.Succeeded)
            {
                return ApiResult.Failure(walletsResult.Error);
            }

            return ApiResult.Ok(walletsResult);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<WalletDto>> Add([FromBody] CreateWalletDto createWalletDto)
        {
            var walletResult = await _mediator.Send(new CreateWalletCommand()
            {
                Wallet = _mapper.Map<Wallet>(createWalletDto)
            });

            if (!walletResult.Succeeded)
            {
                return ApiResult.Failure(walletResult.Error);
            }

            return ApiResult.Ok(walletResult);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<WalletDto>> Update(int id, [FromBody] UpdateWalletDto updateWalletDto)
        {
            var walletResult = await _mediator.Send(new UpdateWalletCommand()
            {
                Id = id,
                CurrencyAmount = updateWalletDto.CurrencyAmount
            });

            if (!walletResult.Succeeded)
            {
                return ApiResult.Failure(walletResult.Error);
            }

            return ApiResult.Ok(walletResult);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult> Delete(int id)
        {
            var walletResult =  await _mediator.Send(new DeleteWalletCommand() { Id = id });

            if (!walletResult.Succeeded)
            {
                return ApiResult.Failure(walletResult.Error);
            }

            return ApiResult.Ok();
        }
    }
}

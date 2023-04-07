using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using UserManagement.Core.Models;
using UserManagement.Core.Services;
using UserManagement.Core.Dtos.User;
using UserManagement.Core.ErrorHandling;

namespace UserManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IRegistrationService _registrationService;
        private readonly IMapper _mapper;

        public UsersController(
            IUserService userService,
            IAuthenticationService authenticationService,
            IRegistrationService registrationService,
            IMapper mapper)
        {
            _userService = userService;
            _authenticationService = authenticationService;
            _registrationService = registrationService;
            _mapper = mapper;
        }

        [HttpPost("registration")]
        [AllowAnonymous]
        public async Task<ApiResult<JWTToken>> Registration([FromBody] RegisterUserDto registerUserDto)
        {
            var tokenResult = await _registrationService.Registration(registerUserDto);

            if (!tokenResult.Succeeded)
            {
                return ApiResult.Failure(tokenResult.Error);
            }

            return ApiResult.Ok(tokenResult);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ApiResult<JWTToken>> Login([FromBody] LoginUserDto loginUserDto)
        {
            var tokenResult = await _authenticationService
                .Authentication(loginUserDto);

            if (!tokenResult.Succeeded)
            {
                return ApiResult.Failure(tokenResult.Error);
            }
            return ApiResult.Ok(tokenResult);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<IEnumerable<UserDto>>> GetAll()
        {
            var usersResult = await _userService.GetAllAsync();
            if (!usersResult.Succeeded)
            {
                return ApiResult.Failure(usersResult.Error);
            }

            return ApiResult.Ok(_mapper.Map<IEnumerable<UserDto>>(usersResult.ValueOrDefault));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<UserDetailsDto>> GetById(int id)
        {
            var userResult = await _userService.GetByIdAsync(id);

            if (!userResult.Succeeded)
            {
                return ApiResult.Failure(userResult.Error);
            }

            return ApiResult.Ok(_mapper.Map<UserDetailsDto>(userResult.Value));
        }

        [HttpPut("{id}")]
        public async Task<ApiResult<UserDto>> Update(int id, [FromBody] UpdateUserDto updateUserDto)
        {
            var userResult = await _userService.GetCheckedUser(
                id,
                User.IsInRole("Admin"),
                User.Identity.Name);

            if (!userResult.Succeeded)
            {
                return ApiResult.Failure(userResult.Error);
            }

            var user = _userService.SetPropertiesForUpdate(userResult.Value, updateUserDto);

            var updatedUser = await _userService.UpdateAsync(user);

            return ApiResult.Ok(_mapper.Map<UserDto>(updatedUser));
        }

        [HttpPut("ChangePassword")]
        public async Task<ApiResult<UserDto>> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            var userResult = await _userService.ChangePassword(User.Identity.Name, changePasswordDto);


            if (!userResult.Succeeded)
            {
                return ApiResult.Failure(userResult.Error);
            }

            return ApiResult.Ok(_mapper.Map<UserDto>(userResult.Value));
        }

        [HttpDelete("{id}")]
        public async Task<ApiResult> Delete(int id)
        {
            var userResult = await _userService.GetCheckedUser(
                id,
                User.IsInRole("Admin"),
                User.Identity.Name);

            if (!userResult.Succeeded)
            {
                return ApiResult.Failure(userResult.Error);
            }

            await _userService.RemoveAsync(userResult.Value);
            return ApiResult.Ok();
        }
    }
}

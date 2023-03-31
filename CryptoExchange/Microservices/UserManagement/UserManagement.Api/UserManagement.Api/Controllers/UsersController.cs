using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using UserManagement.Core.Models;
using UserManagement.Core.Services;
using UserManagement.Core.Dtos.User;

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
        public async Task<IActionResult> Registration([FromBody] RegisterUserDto registerUserDto)
        {
            var token = await _registrationService.Registration(registerUserDto);

            if (token == null) return BadRequest("User already exists");

            return Ok(token);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginUserDto)
        {
            var token = await _authenticationService
                .Authentication(loginUserDto);

            if (token == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(token);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<UserDto>>(users));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);

            return user == null ? NotFound() : Ok(_mapper.Map<UserDetailsDto>(user));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto updateUserDto)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return BadRequest("User not found");

            if (User.IsInRole("User") && user.UserName != User.Identity.Name)
                return BadRequest();

            user.FirstName = updateUserDto.FirstName;
            user.LastName = updateUserDto.LastName;

            var updatedUser = await _userService.UpdateAsync(user);

            return Ok(_mapper.Map<UserDto>(updatedUser));
        }

        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            var user = await _userService.ChangePassword(User.Identity.Name, changePasswordDto);

            if (user == null) return BadRequest("Wrong old password");

            return Ok(_mapper.Map<UserDto>(user));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return BadRequest("User not found");

            if (User.IsInRole("User") && user.UserName != User.Identity.Name)
                return BadRequest();

            await _userService.RemoveAsync(user);
            return Ok();
        }
    }
}

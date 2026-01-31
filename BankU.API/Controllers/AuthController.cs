using BankUAPI.Application.Interface;
using BankUAPI.SharedKernel.Request.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankU.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public readonly IUserRepository _userRepository;
        public AuthController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            var response = await _userRepository.LoginAsync(request);
            return Ok(new { data = response });
        }
    }
}

using BookingService.Api.Services.Interfaces;
using BookingService.Domain.Requests;
using BookingService.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookingService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthTokenService _authTokenService;

        public AccountController(IAuthTokenService authTokenService, IUserRepository userRepository)
        {
            _authTokenService = authTokenService;
            _userRepository = userRepository;
        }

        [HttpPost]
        [Route("signin")]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn(SignInRequest request)
        {
            var user = await _userRepository.Get(request.Username, request.Password);

            if (user == null)
                return NotFound(new { message = "Invalid username or password." });

            var token = _authTokenService.GenerateAuthToken(user);

            user.Password = string.Empty;

            return Ok(new
            {
                user,
                token
            });
        }

        [HttpGet]
        [Route("anonymous")]
        [AllowAnonymous]
        public string Anonymous() => "Anonymous";

        [HttpGet]
        [Route("authenticated")]
        [Authorize]
        public string Authenticated() => $"Authenticated : {User.Identity.Name}";

        [HttpGet]
        [Route("customer")]
        [Authorize(Roles = "Customer,Admin")]
        public string Customer() => $"Authenticated : {User.Identity.Name}";

        [HttpGet]
        [Route("admin")]
        [Authorize(Roles = "Admin")]
        public string Admin() => $"Authenticated : {User.Identity.Name}";
    }
}

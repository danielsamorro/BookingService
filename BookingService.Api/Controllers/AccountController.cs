using BookingService.Api.Services.Interfaces;
using BookingService.Domain.Commands.Requests;
using BookingService.Infrastructure.Repositories.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookingService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IMediator _mediator;
        private readonly IUserRepository _userRepository;
        private readonly IAuthTokenService _authTokenService;

        public AccountController(IMediator signUpHandler, IAuthTokenService authTokenService, IUserRepository userRepository)
        {
            _mediator = signUpHandler;
            _authTokenService = authTokenService;
            _userRepository = userRepository;
        }

        [HttpPost]
        [Route("signup")]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequest request)
        {
            return Ok(await _mediator.Send(request));
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
    }
}

using BookingService.Api.Services.Interfaces;
using BookingService.Domain.Entities;
using BookingService.Domain.Requests;
using BookingService.Domain.SeedWorking.Interfaces;
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
        private readonly IUnitOfWork _unitOfWork;

        public AccountController(IAuthTokenService authTokenService, IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _authTokenService = authTokenService;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Route("signup")]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn(SignUpRequest request)
        {
            var userExists = await _userRepository.CheckDuplicate(request.UserName, request.EmailAddress);

            if (userExists)
                return NotFound(new { message = "Could not sign up user." });

            var user = new User
            {
                EmailAddress = request.EmailAddress,
                Username = request.UserName,
                Password = request.Password,
                Role = "Customer"
            };

            _userRepository.Add(user);

            await _unitOfWork.SaveChangesAsync();

            return Ok(new { message = "User created." });
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

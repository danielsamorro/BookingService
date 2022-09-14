using BookingService.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BookingService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public ReservationController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        [Route("reservations")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetReservations(Guid userId)
        {
            var user = await _userRepository.Get(User.Identity.Name);

            if (user == null)
                return NotFound(new { message = "User not found" });

            if (user.Id != userId)
                return BadRequest(new { message = "Incorrect user provided" });

            return Ok(new
            {
                user.Reservations
            });
        }
    }
}

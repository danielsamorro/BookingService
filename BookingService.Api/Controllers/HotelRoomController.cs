using BookingService.Domain.Requests;
using BookingService.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace BookingService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelRoomController : ControllerBase
    {
        private readonly IHotelRoomRepository _hotelRoomRepository;

        public HotelRoomController(IHotelRoomRepository hotelRoomRepository)
        {
            _hotelRoomRepository = hotelRoomRepository;
        }

        [HttpGet]
        [Route("reserveddates")]
        [AllowAnonymous]
        public async Task<IActionResult> GetReservedDates(GetReservedDatesRequest request)
        {
            var hotelRoom = await _hotelRoomRepository.Get(request.RoomNumber);

            if (hotelRoom == null)
                return NotFound(new { message = "Room not found" });

            return Ok(new
            {
                reservedDates = hotelRoom.Reservations.SelectMany(r => r.ReservationDates).Select(rd => rd.ReservedOn)
            });
        }
    }
}

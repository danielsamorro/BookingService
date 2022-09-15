using BookingService.Api.Extensions;
using BookingService.Domain.Commands.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookingService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelRoomController : ControllerBase
    {
        private readonly IMediator _mediator;

        public HotelRoomController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("GetRooms")]
        [AllowAnonymous]
        public async Task<IActionResult> GetRooms()
        {
            return (await _mediator.Send(new GetRoomsRequest())).ToActionResult();
        }

        [HttpGet]
        [Route("GetReservedDates")]
        [AllowAnonymous]
        public async Task<IActionResult> GetReservedDates(string roomNumber)
        {
            return (await _mediator.Send(new GetReservedDatesRequest { RoomNumber = roomNumber })).ToActionResult();
        }
    }
}

using BookingService.Domain.Commands.Requests;
using MediatR;
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
        private readonly IMediator _mediator;

        public ReservationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("GetReservations")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetReservations(Guid userId)
        {
            return Ok(await _mediator.Send(new GetReservationsRequest { UserId = userId, Username = User.Identity.Name }));
        }

        [HttpPost]
        [Route("CreateReservation")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CreateReservation(CreateReservationRequest request)
        {
            request.Username = User.Identity.Name;

            return Ok(await _mediator.Send(request));
        }

        [HttpPut]
        [Route("ChangeReservation")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> ChangeReservation(ChangeReservationRequest request)
        {
            request.Username = User.Identity.Name;

            return Ok(await _mediator.Send(request));
        }

        [HttpDelete]
        [Route("CancelReservation")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CancelReservation(CancelReservationRequest request)
        {
            request.Username = User.Identity.Name;

            return Ok(await _mediator.Send(request));
        }

    }
}

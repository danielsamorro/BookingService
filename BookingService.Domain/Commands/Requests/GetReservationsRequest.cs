using BookingService.Domain.Responses;
using MediatR;
using System;

namespace BookingService.Domain.Commands.Requests
{
    public class GetReservationsRequest : IRequest<Response>
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
    }
}

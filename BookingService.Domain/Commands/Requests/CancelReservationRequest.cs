using BookingService.Domain.Responses;
using MediatR;
using System;
using System.Text.Json.Serialization;

namespace BookingService.Domain.Commands.Requests
{
    public class CancelReservationRequest : IRequest<Response>
    {
        public Guid UserId { get; set; }
        public Guid ReservationId { get; set; }
        [JsonIgnore]
        public string Username { get; set; }
    }
}

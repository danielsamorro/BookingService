using BookingService.Domain.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BookingService.Domain.Commands.Requests
{
    public class ChangeReservationRequest : IRequest<Response>
    {
        public Guid UserId { get; set; }
        public Guid ReservationId { get; set; }
        [JsonIgnore]
        public string Username { get; set; }
        public List<DateTime> Dates { get; set; }
    }
}

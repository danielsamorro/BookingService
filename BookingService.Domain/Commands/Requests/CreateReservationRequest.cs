using BookingService.Domain.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BookingService.Domain.Commands.Requests
{
    public class CreateReservationRequest : IRequest<Response>
    {
        public Guid UserId { get; set; }
        public Guid HotelRoomId { get; set; }
        [JsonIgnore]
        public string Username { get; set; }
        public List<DateTime> Dates { get; set; }
    }
}

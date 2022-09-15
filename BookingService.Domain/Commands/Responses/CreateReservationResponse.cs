using BookingService.Domain.Responses;
using MediatR;
using System;
using System.Collections.Generic;

namespace BookingService.Domain.Commands.Responses
{
    public class CreateReservationResponse : IRequest<Response>
    {
        public CreateReservationResponse()
        {
            Dates = new List<DateTime>();
        }

        public string Message { get; set; }
        public Guid ReservationId { get; set; }
        public Guid UserId { get; set; }
        public Guid HotelRoomId { get; set; }
        public List<DateTime> Dates { get; set; }
    }
}

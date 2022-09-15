using BookingService.Domain.Dtos;
using System.Collections.Generic;

namespace BookingService.Domain.Commands.Responses
{
    public class GetReservationsResponse
    {
        public GetReservationsResponse()
        {
            Reservations = new List<ReservationDto>();
        }

        public List<ReservationDto> Reservations{ get; set; }
    }
}

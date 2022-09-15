using System;
using System.Collections.Generic;

namespace BookingService.Domain.Dtos
{
    public class ReservationDto
    {
        public ReservationDto()
        {
            Dates = new List<DateTime>();
        }

        public Guid UserId { get; set; }
        public Guid HotelRoomId { get; set; }
        public Guid ReservationId { get; set; }
        public List<DateTime> Dates { get; set; }
    }
}

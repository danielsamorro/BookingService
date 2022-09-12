using System;
using System.Collections.Generic;
using System.Text;

namespace BookingService.Domain.Entities
{
    public class Reservation
    {
        public Guid Id { get; set; }
        public Guid HotelRoomId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public virtual HotelRoom HotelRoom { get; set; }
    }
}

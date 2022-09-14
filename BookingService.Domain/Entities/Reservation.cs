using System;
using System.Collections.Generic;

namespace BookingService.Domain.Entities
{
    public class Reservation
    {
        public Reservation()
        {
            ReservationDates = new List<ReservationDate>();
        }

        public Guid Id { get; set; }
        public Guid HotelRoomId { get; set; }
        public Guid UserId { get; set; }

        public virtual User User { get; set; }
        public virtual HotelRoom HotelRoom { get; set; }
        public virtual List<ReservationDate> ReservationDates { get; set; }
        
    }
}

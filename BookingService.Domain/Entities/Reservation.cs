using System;
using System.Collections.Generic;
using System.Text;

namespace BookingService.Domain.Entities
{
    public class Reservation
    {
        public Guid Id { get; set; }
        public Guid HotelRoomId { get; set; }
        
        public virtual List<ReservationDate> ReservationDates { get; set; }
        public virtual HotelRoom HotelRoom { get; set; }
    }
}

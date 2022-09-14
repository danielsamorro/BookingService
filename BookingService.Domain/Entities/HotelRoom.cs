using System;
using System.Collections.Generic;

namespace BookingService.Domain.Entities
{
    public class HotelRoom
    {
        public HotelRoom()
        {
            Reservations = new List<Reservation>();
        }

        public Guid Id { get; set; }
        public string Number { get; set; }
        public virtual List<Reservation> Reservations { get; set; }
    }
}

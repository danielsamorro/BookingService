﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BookingService.Domain.Entities
{
    public class HotelRoom
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public virtual List<Reservation> Reservations { get; set; }
    }
}
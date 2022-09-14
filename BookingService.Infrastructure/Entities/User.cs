using System;
using System.Collections.Generic;

namespace BookingService.Infrastructure.Entities
{
    public class User
    {
        public User()
        {
            Reservations = new List<Reservation>();
        }

        public Guid Id { get; set; }
        public string EmailAddress { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public virtual List<Reservation> Reservations { get; set; }
    }
}

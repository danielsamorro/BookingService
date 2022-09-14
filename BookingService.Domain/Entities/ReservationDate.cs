using System;
using System.Collections.Generic;
using System.Text;

namespace BookingService.Domain.Entities
{
    public class ReservationDate
    {
        public Guid Id { get; set; }
        public DateTime ReservedOn { get; set; }
        public Guid ReservationId { get; set; }

        public virtual Reservation Reservation { get; set; }
    }
}

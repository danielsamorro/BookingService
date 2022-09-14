using System;

namespace BookingService.Infrastructure.Entities
{
    public class ReservationDate
    {
        public Guid Id { get; set; }
        public DateTime ReservedOn { get; set; }
        public Guid ReservationId { get; set; }

        public virtual Reservation Reservation { get; set; }
    }
}

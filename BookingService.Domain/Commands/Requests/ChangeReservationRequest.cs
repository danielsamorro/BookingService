using System;
using System.Collections.Generic;

namespace BookingService.Domain.Commands.Requests
{
    public class ChangeReservationRequest
    {
        public Guid UserId { get; set; }
        public Guid ReservationId { get; set; }
        public List<DateTime> Dates { get; set; }
    }
}

using System;

namespace BookingService.Domain.Commands.Requests
{
    public class CancelReservationRequest
    {
        public Guid UserId { get; set; }
        public Guid ReservationId { get; set; }
    }
}

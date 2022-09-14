using System;
using System.Collections.Generic;

namespace BookingService.Domain.Requests
{
    public class BookRoomRequest
    {
        public Guid UserId { get; set; }
        public Guid HotelRoomId { get; set; }
        public List<DateTime> Dates { get; set; }
    }
}

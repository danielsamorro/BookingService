using System;
using System.Collections.Generic;
using System.Text;

namespace BookingService.Domain.Requests
{
    public class GetReservedDatesRequest
    {
        public string RoomNumber { get; set; }
    }
}

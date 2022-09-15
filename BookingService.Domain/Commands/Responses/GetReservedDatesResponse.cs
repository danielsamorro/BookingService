using System;
using System.Collections.Generic;

namespace BookingService.Domain.Commands.Responses
{
    public class GetReservedDatesResponse
    {
        public GetReservedDatesResponse()
        {
            ReservedDates = new List<DateTime>();
        }

        public List<DateTime> ReservedDates { get; set; }
    }
}

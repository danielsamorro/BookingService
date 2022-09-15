using System;

namespace BookingService.Domain.Exceptions
{
    public class BadRequestException : ApplicationException
    {
        public BadRequestException(string message) : base(message) { }
    }
}

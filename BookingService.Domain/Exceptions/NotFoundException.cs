using System;

namespace BookingService.Domain.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string mesage) : base(mesage) { }
    }
}

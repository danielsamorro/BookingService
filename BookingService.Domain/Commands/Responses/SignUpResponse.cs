using System;

namespace BookingService.Domain.Commands.Responses
{
    public class SignUpResponse
    {
        public Guid UseId { get; set; }
        public string Username { get; set; }
        public string EmailAddress { get; set; }
    }
}

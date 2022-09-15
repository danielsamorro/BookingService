using System;

namespace BookingService.Domain.Commands.Responses
{
    public class SignInResponse
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string EmailAddress { get; set; }
        public string Token { get; set; }
    }
}

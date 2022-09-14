using System;
using System.Collections.Generic;
using System.Text;

namespace BookingService.Domain.Requests
{
    public class AuthenticateRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}

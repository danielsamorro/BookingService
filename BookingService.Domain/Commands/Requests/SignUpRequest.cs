using BookingService.Domain.Responses;
using MediatR;

namespace BookingService.Domain.Commands.Requests
{
    public class SignUpRequest : IRequest<Response>
    {
        public string EmailAddress { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

    }
}

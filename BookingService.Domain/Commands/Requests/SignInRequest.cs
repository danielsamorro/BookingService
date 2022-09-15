using BookingService.Domain.Responses;
using MediatR;

namespace BookingService.Domain.Commands.Requests
{
    public class SignInRequest : IRequest<Response>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}

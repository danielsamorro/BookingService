using BookingService.Domain.Responses;
using MediatR;

namespace BookingService.Domain.Commands.Requests
{
    public class GetRoomsRequest : IRequest<Response>
    {
    }
}

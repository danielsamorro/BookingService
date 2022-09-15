using BookingService.Domain.Responses;
using MediatR;

namespace BookingService.Domain.Commands.Requests
{
    public class GetReservedDatesRequest : IRequest<Response>
    {
        public string RoomNumber { get; set; }
    }
}

using BookingService.Domain.Dtos;
using System.Collections.Generic;

namespace BookingService.Domain.Commands.Responses
{
    public class GetRoomsResponse
    {
        public List<RoomDto> Rooms { get; set; }
    }
}

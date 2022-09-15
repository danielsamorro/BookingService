using BookingService.Domain.Commands.Requests;
using BookingService.Domain.Commands.Responses;
using BookingService.Domain.Responses;
using BookingService.Infrastructure.Repositories.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BookingService.Domain.Commands.Handlers
{
    public class GetRoomsHandler : IRequestHandler<GetRoomsRequest, Response>
    {
        private readonly IHotelRoomRepository _hotelRoomRepository;

        public GetRoomsHandler(IHotelRoomRepository hotelRoomRepository)
        {
            _hotelRoomRepository = hotelRoomRepository;
        }

        public async Task<Response> Handle(GetRoomsRequest request, CancellationToken cancellationToken)
        {
            Response response;

            try
            {
                var rooms = await _hotelRoomRepository.GetAll();

                response = new Response(new GetRoomsResponse
                {
                    Rooms = rooms.Select(hr => new Dtos.RoomDto
                    {
                        Id = hr.Id,
                        Number = hr.Number
                    }).ToList()
                });
            }
            catch (Exception ex)
            {
                response = new ErrorResponse(ex.Message);
            }

            return response;
        }
    }
}

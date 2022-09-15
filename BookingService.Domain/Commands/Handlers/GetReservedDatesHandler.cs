using BookingService.Domain.Commands.Requests;
using BookingService.Domain.Commands.Responses;
using BookingService.Domain.Exceptions;
using BookingService.Domain.Responses;
using BookingService.Infrastructure.Repositories.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BookingService.Domain.Commands.Handlers
{
    public class GetReservedDatesHandler : IRequestHandler<GetReservedDatesRequest, Response>
    {
        private readonly IHotelRoomRepository _hotelRoomRepository;

        public GetReservedDatesHandler(IHotelRoomRepository hotelRoomRepository)
        {
            _hotelRoomRepository = hotelRoomRepository;
        }

        public async Task<Response> Handle(GetReservedDatesRequest request, CancellationToken cancellationToken)
        {
            Response response;

            try
            {
                var hotelRoom = await _hotelRoomRepository.Get(request.RoomNumber);

                if (hotelRoom == null)
                    throw new NotFoundException("Room not found");

                response = new Response(new GetReservedDatesResponse
                {
                    ReservedDates = hotelRoom.Reservations.SelectMany(r => r.ReservationDates).Select(rd => rd.ReservedOn).ToList()
                });
            }
            catch (NotFoundException ex)
            {
                response = new NotFoundResponse(ex.Message);
            }
            catch (Exception ex)
            {
                response = new ErrorResponse(ex.Message);
            }

            return response;
        }
    }
}

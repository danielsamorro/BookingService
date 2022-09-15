using BookingService.Domain.Commands.Requests;
using BookingService.Domain.Commands.Responses;
using BookingService.Domain.Responses;
using BookingService.Infrastructure.Repositories.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BookingService.Domain.Commands.Handlers
{
    public class GetReservationsHandler : IRequestHandler<GetReservationsRequest, Response>
    {
        private readonly IUserRepository _userRepository;

        public GetReservationsHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Response> Handle(GetReservationsRequest request, CancellationToken cancellationToken)
        {
            Response response;

            try
            {
                var user = await _userRepository.Get(request.Username);

                if (user == null)
                    throw new Exception("User not found");

                if (user.Id != request.UserId)
                    throw new Exception("Incorrect user provided");

                response = new Response(new GetReservationsResponse
                {
                    Reservations = user.Reservations.Select(r => new Dtos.ReservationDto
                    {
                        UserId = r.UserId,
                        HotelRoomId = r.HotelRoomId,
                        ReservationId = r.Id,
                        Dates = r.ReservationDates.Select(rd => rd.ReservedOn.Date).ToList()
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

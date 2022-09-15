using BookingService.Domain.Commands.Requests;
using BookingService.Domain.Commands.Responses;
using BookingService.Domain.Exceptions;
using BookingService.Domain.Responses;
using BookingService.Infrastructure.Entities;
using BookingService.Infrastructure.Repositories.Interfaces;
using BookingService.Infrastructure.SeedWorking.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BookingService.Domain.Commands.Handlers
{
    public class ChangeReservationHandler : IRequestHandler<ChangeReservationRequest, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IHotelRoomRepository _hotelRoomRepository;
        private readonly IReservationRepository _reservationRepository;

        public ChangeReservationHandler(IUnitOfWork unitOfWork, IUserRepository userRepository, IHotelRoomRepository hotelRoomRepository, IReservationRepository reservationRepository)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _hotelRoomRepository = hotelRoomRepository;
            _reservationRepository = reservationRepository;
        }

        public async Task<Response> Handle(ChangeReservationRequest request, CancellationToken cancellationToken)
        {
            Response response;

            try
            {
                if (request.Dates.Count > 3)
                    throw new BadRequestException("Cannot book room for a period longer than 3 days");

                if (request.Dates.OrderBy(d => d).FirstOrDefault().Date <= DateTime.Now.Date)
                    throw new BadRequestException("Reservation must start at least tomorrow");

                if (request.Dates.OrderBy(d => d).FirstOrDefault().Date > DateTime.Now.AddDays(30).Date)
                    throw new BadRequestException("Reservation cannot start more than 30 days away");

                var user = await _userRepository.Get(request.Username);

                if (user == null)
                    throw new NotFoundException("User not found");

                if (user.Id != request.UserId)
                    throw new BadRequestException("Incorrect user provided");

                var reservation = await _reservationRepository.Get(request.ReservationId);

                if (reservation == null)
                    throw new NotFoundException("Reservation not found");

                var room = await _hotelRoomRepository.Get(reservation.HotelRoomId);

                if (room == null)
                    throw new NotFoundException("Room not found.");

                if (room.Reservations.Where(r => r.Id != reservation.Id).SelectMany(r => r.ReservationDates).Any(rd => request.Dates.Any(d => d.Date == rd.ReservedOn.Date)))
                    throw new BadRequestException("Date chosen has already been booked");

                reservation.ReservationDates = request.Dates
                        .Select(d => new ReservationDate
                        {
                            ReservedOn = d
                        })
                        .ToList();

                await _unitOfWork.SaveChangesAsync();

                response = new Response(new ChangeReservationResponse
                {
                    Message = "Reservation changed succesfully",
                    ReservationId = reservation.Id,
                    UserId = reservation.UserId,
                    HotelRoomId = reservation.HotelRoomId,
                    Dates = reservation.ReservationDates.Select(rd => rd.ReservedOn.Date).ToList()
                });
            }
            catch (NotFoundException ex)
            {
                response = new NotFoundResponse(ex.Message);
            }
            catch (BadRequestException ex)
            {
                response = new BadRequestResponse(ex.Message);
            }
            catch (Exception ex)
            {
                response = new ErrorResponse(ex.Message);
            }

            return response;
        }
    }
}

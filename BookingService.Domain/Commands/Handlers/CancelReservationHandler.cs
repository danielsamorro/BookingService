using BookingService.Domain.Commands.Requests;
using BookingService.Domain.Commands.Responses;
using BookingService.Domain.Exceptions;
using BookingService.Domain.Responses;
using BookingService.Infrastructure.Repositories.Interfaces;
using BookingService.Infrastructure.SeedWorking.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BookingService.Domain.Commands.Handlers
{
    public class CancelReservationHandler : IRequestHandler<CancelReservationRequest, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IReservationRepository _reservationRepository;

        public CancelReservationHandler(IUnitOfWork unitOfWork, IUserRepository userRepository, IReservationRepository reservationRepository)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _reservationRepository = reservationRepository;
        }

        public async Task<Response> Handle(CancelReservationRequest request, CancellationToken cancellationToken)
        {
            Response response;

            try
            {
                var user = await _userRepository.Get(request.Username);

                if (user == null)
                    throw new NotFoundException("User not found");

                if (user.Id != request.UserId)
                    throw new BadRequestException("Incorrect user provided");

                var reservation = await _reservationRepository.Get(request.ReservationId);

                if (reservation == null)
                    throw new NotFoundException("Reservation not found");

                user.Reservations.Remove(reservation);

                await _unitOfWork.SaveChangesAsync();

                response = new Response(new CancelReservationResponse
                {
                    Message = "Reservation cancelled successfully"
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

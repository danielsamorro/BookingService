﻿using BookingService.Domain.Commands.Requests;
using BookingService.Domain.Commands.Responses;
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
    public class CreateReservationHandler : IRequestHandler<CreateReservationRequest, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IHotelRoomRepository _hotelRoomRepository;

        public CreateReservationHandler(IUnitOfWork unitOfWork, IUserRepository userRepository, IHotelRoomRepository hotelRoomRepository)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _hotelRoomRepository = hotelRoomRepository;
        }

        public async Task<Response> Handle(CreateReservationRequest request, CancellationToken cancellationToken)
        {
            Response response;

            try
            {
                if (request.Dates.Count > 3)
                   throw new Exception("Cannot book room for a period longer than 3 days");

                if (request.Dates.OrderBy(d => d).FirstOrDefault().Date <= DateTime.Now.Date)
                   throw new Exception("Reservation must start at least tomorrow");

                if (request.Dates.OrderBy(d => d).FirstOrDefault().Date > DateTime.Now.AddDays(30).Date)
                   throw new Exception("Reservation cannot start more than 30 days away");

                var user = await _userRepository.Get(request.Username);

                if (user == null)
                    throw new Exception("User not found");

                if (user.Id != request.UserId)
                   throw new Exception("Incorrect user provided");

                var room = await _hotelRoomRepository.Get(request.HotelRoomId);

                if (room == null)
                    throw new Exception("Room not found.");

                if (room.Reservations.SelectMany(r => r.ReservationDates).Any(rd => request.Dates.Any(d => d.Date == rd.ReservedOn.Date)))
                   throw new Exception("Date chosen has already been booked");

                var reservation = new Reservation
                {
                    HotelRoom = room,
                    User = user,
                    ReservationDates = request.Dates
                        .Select(d => new ReservationDate
                        {
                            ReservedOn = d
                        })
                        .ToList()
                };

                user.Reservations.Add(reservation);

                await _unitOfWork.SaveChangesAsync();

                response = new Response(new CreateReservationResponse
                {
                    Message = "Reservation created succesfully",
                    ReservationId = reservation.Id,
                    UserId = reservation.UserId,
                    HotelRoomId = reservation.HotelRoomId,
                    Dates = reservation.ReservationDates.Select(rd => rd.ReservedOn.Date).ToList()
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

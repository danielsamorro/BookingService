using BookingService.Domain.Entities;
using BookingService.Domain.Requests;
using BookingService.Domain.SeedWorking.Interfaces;
using BookingService.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BookingService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly IHotelRoomRepository _hotelRoomRepository;

        public ReservationController(IUnitOfWork unitOfWork, IUserRepository userRepository, IReservationRepository reservationRepository, IHotelRoomRepository hotelRoomRepository)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _reservationRepository = reservationRepository;
            _hotelRoomRepository = hotelRoomRepository;
        }

        [HttpGet]
        [Route("getreservations")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetReservations(Guid userId)
        {
            var user = await _userRepository.Get(User.Identity.Name);

            if (user == null)
                return NotFound(new { message = "User not found" });

            if (user.Id != userId)
                return BadRequest(new { message = "Incorrect user provided" });

            return Ok(new
            {
                reservations = user.Reservations.Select(r => new { r.UserId, r.HotelRoomId, reservationId = r.Id, dates = r.ReservationDates.Select(rd => rd.ReservedOn.Date) })
            });
        }

        [HttpPost]
        [Route("createreservation")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CreateReservation(ReservationRequest request)
        {
            if (request.Dates.Count > 3)
                return BadRequest(new { message = "Cannot book room for a period longer than 3 days" });

            if (request.Dates.OrderBy(d => d).FirstOrDefault().Date == DateTime.Now.Date)
                return BadRequest(new { message = "Reservation cannot start today" });

            if (request.Dates.OrderBy(d => d).FirstOrDefault().Date > DateTime.Now.AddDays(30).Date)
                return BadRequest(new { message = "Reservation cannot start more than 30 days away" });

            var user = await _userRepository.Get(User.Identity.Name);

            if (user == null)
                return NotFound(new { message = "User not found" });

            if (user.Id != request.UserId)
                return BadRequest(new { message = "Incorrect user provided" });

            var room = await _hotelRoomRepository.Get(request.HotelRoomId);

            if (room == null)
                return NotFound(new { message = "Room not found." });            

            if (room.Reservations.SelectMany(r => r.ReservationDates).Any(rd => request.Dates.Any(d => d.Date == rd.ReservedOn.Date)))
                return BadRequest(new { message = "Date chosen has already been booked" });

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

            return Ok(new
            {
                message = "Reservation created succesfully",
                reservationId = reservation.Id,
                userId = user.Id,
                hotelRoomId = room.Id,
                dates = reservation.ReservationDates.Select(rd => rd.ReservedOn.Date)
            });
        }

        [HttpPut]
        [Route("changeReservation")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> ChangeReservation(ChangeReservationRequest request)
        {
            if (request.Dates.Count > 3)
                return BadRequest(new { message = "Cannot book room for a period longer than 3 days" });

            if (request.Dates.OrderBy(d => d).FirstOrDefault().Date == DateTime.Now.Date)
                return BadRequest(new { message = "Reservation cannot start today" });

            if (request.Dates.OrderBy(d => d).FirstOrDefault().Date > DateTime.Now.AddDays(30).Date)
                return BadRequest(new { message = "Reservation cannot start more than 30 days away" });

            var user = await _userRepository.Get(User.Identity.Name);

            if (user == null)
                return NotFound(new { message = "User not found" });

            if (user.Id != request.UserId)
                return BadRequest(new { message = "Incorrect user provided" });

            var reservation = await _reservationRepository.Get(request.ReservationId);

            if (reservation == null)
                return NotFound(new { message = "Reservation not found" });

            var room = await _hotelRoomRepository.Get(reservation.HotelRoomId);

            if (room.Reservations.Where(r => r.Id != reservation.Id).SelectMany(r => r.ReservationDates).Any(rd => request.Dates.Any(d => d.Date == rd.ReservedOn.Date)))
                return BadRequest(new { message = "Date chosen has already been booked" });

            reservation.ReservationDates = request.Dates
                    .Select(d => new ReservationDate
                    {
                        ReservedOn = d
                    })
                    .ToList();

            await _unitOfWork.SaveChangesAsync();

            return Ok(new
            {
                message = "Reservation updated succesfully",
                reservationId = reservation.Id,
                userId = user.Id,
                hotelRoomId = room.Id,
                dates = reservation.ReservationDates.Select(rd => rd.ReservedOn.Date)
            });
        }

        [HttpDelete]
        [Route("cancelReservation")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CancelReservation(CancelReservationRequest request)
        {
            var user = await _userRepository.Get(User.Identity.Name);

            if (user == null)
                return NotFound(new { message = "User not found" });

            if (user.Id != request.UserId)
                return BadRequest(new { message = "Incorrect user provided" });

            var reservation = await _reservationRepository.Get(request.ReservationId);

            if (reservation == null)
                return NotFound(new { message = "Reservation not found" });

            user.Reservations.Remove(reservation);

            await _unitOfWork.SaveChangesAsync();

            return Ok(new
            {
                message = "Reservation cancelled successfully"
            });

        }

    }
}

using BookingService.Infrastructure.Entities;
using BookingService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookingService.Infrastructure.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly BookingServiceContext _context;

        public ReservationRepository(BookingServiceContext context)
        {
            _context = context;
        }

        public void Add(Reservation reservation)
        {
            _context.Reservations.Add(reservation);
        }

        public async Task<Reservation> Get(Guid id)
        {
            return await _context.Reservations
                .Include(r => r.ReservationDates)
                .Include(r => r.HotelRoom)
                .FirstOrDefaultAsync(h => h.Id == id);
        }

        public async Task<IEnumerable<Reservation>> GetAll()
        {
            return await _context.Reservations.ToListAsync();
        }
    }
}

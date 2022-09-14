using BookingService.Domain.Entities;
using BookingService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public Reservation Get(Guid id)
        {
            return _context.Reservations.FirstOrDefault(h => h.Id == id);
        }

        public async Task<IEnumerable<Reservation>> GetAll()
        {
            return await _context.Reservations.ToListAsync();
        }
    }
}

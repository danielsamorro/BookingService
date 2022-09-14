using BookingService.Domain.Entities;
using BookingService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookingService.Infrastructure.Repositories
{
    public class HotelRoomRepository : IHotelRoomRepository
    {
        private readonly BookingServiceContext _context;

        public HotelRoomRepository(BookingServiceContext context)
        {
            _context = context;
        }

        public void Add(HotelRoom hotelRoom)
        {
            _context.HotelRooms.Add(hotelRoom);
        }

        public async Task<HotelRoom> Get(Guid id)
        {
            return await _context.HotelRooms
                .Include(h => h.Reservations)
                .ThenInclude(r => r.ReservationDates)
                .FirstOrDefaultAsync(h => h.Id == id);
        }

        public async Task<HotelRoom> Get(string number)
        {
            return await _context.HotelRooms
                .Include(h => h.Reservations)
                .ThenInclude(r => r.ReservationDates)
                .FirstOrDefaultAsync(h => h.Number == number);
        }

        public async Task<IEnumerable<HotelRoom>> GetAll()
        {
            return await _context.HotelRooms.ToListAsync();
        }
    }
}

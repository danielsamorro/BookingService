using BookingService.Domain.Entities;
using BookingService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public HotelRoom Get(Guid id)
        {
            return _context.HotelRooms.FirstOrDefault(h => h.Id == id);
        }

        public HotelRoom Get(string number)
        {
            return _context.HotelRooms.FirstOrDefault(h => h.Number == number);
        }

        public async Task<IEnumerable<HotelRoom>> GetAll()
        {
            return await _context.HotelRooms.ToListAsync();
        }
    }
}

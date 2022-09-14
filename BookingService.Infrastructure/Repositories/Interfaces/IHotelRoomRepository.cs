using BookingService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookingService.Infrastructure.Repositories.Interfaces
{
    public interface IHotelRoomRepository
    {
        void Add(HotelRoom hotelRoom);
        Task<HotelRoom> Get(Guid id);
        Task<HotelRoom> Get(string number);
        Task<IEnumerable<HotelRoom>> GetAll();
    }
}

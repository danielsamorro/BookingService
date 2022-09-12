using BookingService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Infrastructure.Repositories.Interfaces
{
    public interface IHotelRoomRepository
    {
        void Add(HotelRoom hotelRoom);
        HotelRoom Get(Guid id);
        HotelRoom Get(string number);
        Task<IEnumerable<HotelRoom>> GetAll();
    }
}

using BookingService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookingService.Infrastructure.Repositories.Interfaces
{
    public interface IReservationRepository
    {
        void Add(Reservation hotelRoom);
        Task<Reservation> Get(Guid id);
        Task<IEnumerable<Reservation>> GetAll();
    }
}

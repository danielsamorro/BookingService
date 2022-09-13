using BookingService.Domain.Entities;

namespace BookingService.Infrastructure.Repositories.Interfaces
{
    public interface IUserRepository
    {
        User Get(string username, string password);
    }
}

using BookingService.Domain.Entities;
using System.Threading.Tasks;

namespace BookingService.Infrastructure.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> Get(string username, string password);
        Task<User> Get(string username);
    }
}

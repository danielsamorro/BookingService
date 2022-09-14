using BookingService.Infrastructure.Entities;
using System.Threading.Tasks;

namespace BookingService.Infrastructure.Repositories.Interfaces
{
    public interface IUserRepository
    {
        void Add(User user);
        Task<User> Get(string username, string password);
        Task<User> Get(string username);
        Task<bool> CheckDuplicate(string username, string emailAddress);
    }
}

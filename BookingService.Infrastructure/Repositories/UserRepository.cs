using BookingService.Domain.Entities;
using BookingService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BookingService.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BookingServiceContext _context;

        public UserRepository(BookingServiceContext context)
        {
            _context = context;
        }

        public async Task<User> Get(string username, string password)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username.Equals(username) && u.Password.Equals(password));
        }

        public async Task<User> Get(string username)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username.Equals(username));
        }
    }
}

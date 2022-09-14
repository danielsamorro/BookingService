using BookingService.Infrastructure.Entities;
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

        public void Add(User user)
        {
            _context.Users.Add(user);
        }

        public async Task<User> Get(string username, string password)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username.Equals(username) && u.Password.Equals(password));
        }

        public async Task<User> Get(string username)
        {
            return await _context.Users
                .Include(u => u.Reservations)
                .ThenInclude(r => r.ReservationDates)
                .FirstOrDefaultAsync(u => u.Username.Equals(username));
        }

        public async Task<bool> CheckDuplicate(string username, string emailAddress)
        {
            return await _context.Users.AnyAsync(u => u.Username == username || u.EmailAddress == emailAddress);
        }
    }
}

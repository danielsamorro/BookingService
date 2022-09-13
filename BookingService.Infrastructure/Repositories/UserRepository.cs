using BookingService.Domain.Entities;
using BookingService.Infrastructure.Repositories.Interfaces;
using System;
using System.Linq;

namespace BookingService.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BookingServiceContext _context;

        public UserRepository(BookingServiceContext context)
        {
            _context = context;
        }

        public User Get(string username, string password)
        {
            return _context.Users.FirstOrDefault(u => u.Username.Equals(username) && u.Password.Equals(password));
        }
    }
}

using BookingService.Infrastructure;
using BookingService.Infrastructure.Entities;
using BookingService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Xunit;

namespace BookingService.Test.Infrastructure.Repositories
{
    public class UserRepositoryTests
    {
        private BookingServiceContext _context;
        private UserRepository _repository;

        public UserRepositoryTests()
        {
            
        }

        [Fact]
        public async Task Add_Should_Add_To_Context()
        {
            CreateDataBase("UserRepositoryTest1");

            var user = new User
            {
                EmailAddress = "test@test.com",
                Username = "testuser",
                Role = "Customer",
                Password = "jifgnergenrg"
            };

            _repository.Add(user);
            await _context.SaveChangesAsync();

            Assert.NotEmpty(_context.Users);
        }

        [Fact]
        public async Task Get_User_Pswd_Should_Return_User()
        {
            CreateDataBase("UserRepositoryTest2");

            var user = new User
            {
                EmailAddress = "test@test.com",
                Username = "testuser",
                Role = "Customer",
                Password = "jifgnergenrg"
            };

            var dbEntity = _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var fetched = await _repository.Get(user.Username, user.Password);

            Assert.NotNull(fetched);
            Assert.IsType<User>(dbEntity.Entity);
            Assert.Equal(dbEntity.Entity.Id, fetched.Id);
        }

        [Fact]
        public async Task Get_Should_Return_User()
        {
            CreateDataBase("UserRepositoryTest3");

            var user = new User
            {
                EmailAddress = "test@test.com",
                Username = "testuser",
                Role = "Customer",
                Password = "jifgnergenrg"
            };

            var dbEntity = _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var fetched = await _repository.Get(user.Username);

            Assert.NotNull(fetched);
            Assert.IsType<User>(dbEntity.Entity);
            Assert.Equal(dbEntity.Entity.Id, fetched.Id);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task CheckDuplicate_Should_Return(bool isDuplicate)
        {
            CreateDataBase("UserRepositoryTest4");

            var user = new User
            {
                EmailAddress = "test@test.com",
                Username = "testuser",
                Role = "Customer",
                Password = "jifgnergenrg"
            };

            var dbEntity = _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var result = await _repository.CheckDuplicate(isDuplicate ? user.Username : "testuser2", isDuplicate ? user.EmailAddress : "test2@test.com");

            Assert.IsType<bool>(result);
            Assert.Equal(isDuplicate, result);
        }

        private void CreateDataBase(string name)
        {
            var dbOptions = new DbContextOptionsBuilder<BookingServiceContext>()
                .UseInMemoryDatabase(name)
                .Options;

            _context = new BookingServiceContext(dbOptions);
            _repository = new UserRepository(_context);
        }
    }
}

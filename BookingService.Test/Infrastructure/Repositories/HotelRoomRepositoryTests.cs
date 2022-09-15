using BookingService.Infrastructure;
using BookingService.Infrastructure.Entities;
using BookingService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BookingService.Test.Infrastructure.Repositories
{
    public class HotelRoomRepositoryTests
    {
        private BookingServiceContext _context;
        private HotelRoomRepository _repository;

        public HotelRoomRepositoryTests()
        {
        }

        [Fact]
        public async Task Add_Should_Add_To_Context()
        {
            CreateDataBase("HotelRoomRepositoryTest1");

            var hotelRoom = new HotelRoom
            {
                Number = "11"
            };

            _repository.Add(hotelRoom);
            await _context.SaveChangesAsync();

            Assert.Contains(hotelRoom.Number, _context.HotelRooms.ToList().Select(h => h.Number));
        }

        [Fact]
        public async Task Get_Should_Return_HotelRoom()
        {
            CreateDataBase("HotelRoomRepositoryTest2");

            var hotelRoom = new HotelRoom
            {
                Number = "11"
            };

            var dbRoom = _context.HotelRooms.Add(hotelRoom);
            await _context.SaveChangesAsync();

            var fetchedRoom = await _repository.Get(hotelRoom.Id);

            Assert.NotNull(fetchedRoom);
            Assert.IsType<HotelRoom>(dbRoom.Entity);
            Assert.Equal(dbRoom.Entity.Id, fetchedRoom.Id);
        }

        [Fact]
        public async Task Get_By_Number_Should_Return_HotelRoom()
        {
            CreateDataBase("HotelRoomRepositoryTest3");

            var hotelRoom = new HotelRoom
            {
                Number = "11"
            };

            var dbRoom = _context.HotelRooms.Add(hotelRoom);
            await _context.SaveChangesAsync();

            var fetchedRoom = await _repository.Get(hotelRoom.Number);

            Assert.NotNull(fetchedRoom);
            Assert.IsType<HotelRoom>(dbRoom.Entity);
            Assert.Equal(dbRoom.Entity.Number, fetchedRoom.Number);
        }

        [Fact]
        public async Task GetAll_Should_Return_All_HotelRooms()
        {
            CreateDataBase("HotelRoomRepositoryTest4");

            var rooms = new List<HotelRoom>
            {
                new HotelRoom { Number = "11" },
                new HotelRoom { Number = "12" },
                new HotelRoom { Number = "13" },
            };

            _context.HotelRooms.AddRange(rooms);
            await _context.SaveChangesAsync();

            var fetchedRooms = await _repository.GetAll();

            Assert.NotNull(fetchedRooms);
            Assert.IsType<List<HotelRoom>>(fetchedRooms);
            Assert.Equal(rooms.Count, fetchedRooms.ToList().Count);
        }

        private void CreateDataBase(string name)
        {
            var dbOptions = new DbContextOptionsBuilder<BookingServiceContext>()
                .UseInMemoryDatabase(name)
                .Options;

            _context = new BookingServiceContext(dbOptions);
            _repository = new HotelRoomRepository(_context);
        }
    }
}

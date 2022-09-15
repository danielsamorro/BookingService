using BookingService.Infrastructure;
using BookingService.Infrastructure.Entities;
using BookingService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BookingService.Test.Infrastructure.Repositories
{
    public class ReservationRepositoryTests
    {
        private BookingServiceContext _context;
        private ReservationRepository _repository;

        public ReservationRepositoryTests()
        {
            
        }

        [Fact]
        public async Task Add_Should_Add_To_Context()
        {
            CreateDataBase("ReservationRepositoryTest1");

            var user = new User
            {
                EmailAddress = "test@test.com",
                Username = "testuser",
                Role = "Customer",
                Password = "jifgnergenrg"
            };
            var room = new HotelRoom
            {
                Number = "11"
            };

            var reservation = new Reservation
            {
                HotelRoom = room,
                User = user,
                ReservationDates = new List<ReservationDate>
                {
                    new ReservationDate
                    {
                        ReservedOn = DateTime.Now
                    }
                }
            };

            _repository.Add(reservation);
            await _context.SaveChangesAsync();

            Assert.NotEmpty(_context.ReservationDates);
        }

        [Fact]
        public async Task Get_Should_Return_Reservation()
        {
            CreateDataBase("ReservationRepositoryTest2");

            var user = new User
            {
                EmailAddress = "test@test.com",
                Username = "testuser",
                Role = "Customer",
                Password = "jifgnergenrg"
            };
            var room = new HotelRoom
            {
                Number = "11"
            };

            var reservation = new Reservation
            {
                HotelRoom = room,
                User = user,
                ReservationDates = new List<ReservationDate>
                {
                    new ReservationDate
                    {
                        ReservedOn = DateTime.Now
                    }
                }
            };

            var dbEntity = _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            var fetched = await _repository.Get(reservation.Id);

            Assert.NotNull(fetched);
            Assert.IsType<Reservation>(dbEntity.Entity);
            Assert.Equal(dbEntity.Entity.Id, fetched.Id);
        }

        [Fact]
        public async Task GetAll_Should_Return_All_HotelRooms()
        {
            CreateDataBase("ReservationRepositoryTest3");

            var user = new User
            {
                EmailAddress = "test@test.com",
                Username = "testuser",
                Role = "Customer",
                Password = "jifgnergenrg"
            };
            var room = new HotelRoom
            {
                Number = "11"
            };

            var reservations = new List<Reservation>();

            for (int i = 0; i < 3; i++)
            {
                var reservation = new Reservation
                {
                    HotelRoom = room,
                    User = user,
                    ReservationDates = new List<ReservationDate>
                    {
                        new ReservationDate
                        {
                            ReservedOn = DateTime.Now
                        }
                    }
                };

                reservations.Add(reservation);
            }

            _context.Reservations.AddRange(reservations);
            await _context.SaveChangesAsync();

            var fetchedRooms = await _repository.GetAll();

            Assert.NotNull(fetchedRooms);
            Assert.IsType<List<Reservation>>(fetchedRooms);
            Assert.Equal(reservations.Count, fetchedRooms.ToList().Count);
        }

        private void CreateDataBase(string name)
        {
            var dbOptions = new DbContextOptionsBuilder<BookingServiceContext>()
                .UseInMemoryDatabase(name)
                .Options;

            _context = new BookingServiceContext(dbOptions);
            _repository = new ReservationRepository(_context);
        }
    }
}

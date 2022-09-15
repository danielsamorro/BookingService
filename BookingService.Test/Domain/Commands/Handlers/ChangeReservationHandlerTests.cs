using BookingService.Domain.Commands.Handlers;
using BookingService.Domain.Commands.Requests;
using BookingService.Domain.Commands.Responses;
using BookingService.Domain.Responses;
using BookingService.Infrastructure.Entities;
using BookingService.Infrastructure.Repositories.Interfaces;
using BookingService.Infrastructure.SeedWorking.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace BookingService.Test.Domain.Commands.Handlers
{
    public class ChangeReservationHandlerTests
    {
        private readonly MockRepository _mockRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IHotelRoomRepository> _mockHotelRoomRepository;
        private readonly Mock<IReservationRepository> _mockReservationRepository;

        private readonly ChangeReservationHandler _handler;

        public ChangeReservationHandlerTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _mockUnitOfWork = _mockRepository.Create<IUnitOfWork>();
            _mockUserRepository = _mockRepository.Create<IUserRepository>();
            _mockHotelRoomRepository = _mockRepository.Create<IHotelRoomRepository>();
            _mockReservationRepository = _mockRepository.Create<IReservationRepository>();

            _handler = new ChangeReservationHandler(_mockUnitOfWork.Object, _mockUserRepository.Object, _mockHotelRoomRepository.Object, _mockReservationRepository.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_ErrorResponse()
        {
            var request = new ChangeReservationRequest
            {
                ReservationId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Username = "testuser",
                Dates = new List<DateTime>
                {
                    DateTime.Now.AddDays(1)
                }                
            };

            _mockUserRepository.Setup(m => m.Get(It.Is<string>(u => u.Equals(request.Username)))).Throws(new Exception("error"));

            var result = await _handler.Handle(request, default);

            Assert.NotNull(result);
            Assert.IsType<ErrorResponse>(result);
            Assert.NotEmpty(result.Errors);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Handle_Should_Return_BadRequestResponse_Period_Longer_Than_3_Days()
        {
            var request = new ChangeReservationRequest
            {
                ReservationId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Username = "testuser",
                Dates = new List<DateTime>
                {
                    DateTime.Now.AddDays(1),
                    DateTime.Now.AddDays(2),
                    DateTime.Now.AddDays(3),
                    DateTime.Now.AddDays(4)
                }
            };

            var result = await _handler.Handle(request, default);

            Assert.NotNull(result);
            Assert.IsType<BadRequestResponse>(result);
            Assert.NotEmpty(result.Errors);
            Assert.Equal("Cannot book room for a period longer than 3 days", result.Errors.FirstOrDefault());

            _mockRepository.VerifyAll();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Handle_Should_Return_BadRequestResponse_Starts_Today_Or_Before(bool startsToday)
        {
            var request = new ChangeReservationRequest
            {
                ReservationId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Username = "testuser",
                Dates = new List<DateTime>
                {
                    startsToday ? DateTime.Now : DateTime.Now.AddDays(-1)
                }
            };

            var result = await _handler.Handle(request, default);

            Assert.NotNull(result);
            Assert.IsType<BadRequestResponse>(result);
            Assert.NotEmpty(result.Errors);
            Assert.Equal("Reservation must start at least tomorrow", result.Errors.FirstOrDefault());

            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Handle_Should_Return_BadRequestResponse_More_Than_30_Days_Away()
        {
            var request = new ChangeReservationRequest
            {
                ReservationId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Username = "testuser",
                Dates = new List<DateTime>
                {
                    DateTime.Now.AddDays(31)
                }
            };

            var result = await _handler.Handle(request, default);

            Assert.NotNull(result);
            Assert.IsType<BadRequestResponse>(result);
            Assert.NotEmpty(result.Errors);
            Assert.Equal("Reservation cannot start more than 30 days away", result.Errors.FirstOrDefault());

            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Handle_Should_Return_NotFoundResponse_User_Not_Found()
        {
            var request = new ChangeReservationRequest
            {
                ReservationId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Username = "testuser",
                Dates = new List<DateTime>
                {
                    DateTime.Now.AddDays(1)
                }
            };

            _mockUserRepository.Setup(m => m.Get(It.Is<string>(u => u.Equals(request.Username)))).ReturnsAsync((User)null);

            var result = await _handler.Handle(request, default);

            Assert.NotNull(result);
            Assert.IsType<NotFoundResponse>(result);
            Assert.NotEmpty(result.Errors);
            Assert.Equal("User not found", result.Errors.FirstOrDefault());

            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Handle_Should_Return_BadRequestResponse_Incorrect_User()
        {
            var request = new ChangeReservationRequest
            {
                ReservationId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Username = "testuser",
                Dates = new List<DateTime>
                {
                    DateTime.Now.AddDays(1)
                }
            };
            var user = new User
            {
                Id = Guid.NewGuid()
            };

            _mockUserRepository.Setup(m => m.Get(It.Is<string>(u => u.Equals(request.Username)))).ReturnsAsync(user);

            var result = await _handler.Handle(request, default);

            Assert.NotNull(result);
            Assert.IsType<BadRequestResponse>(result);
            Assert.NotEmpty(result.Errors);
            Assert.Equal("Incorrect user provided", result.Errors.FirstOrDefault());

            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Handle_Should_Return_NotFoundResponse_Reservation_Not_Found()
        {
            var request = new ChangeReservationRequest
            {
                ReservationId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Username = "testuser",
                Dates = new List<DateTime>
                {
                    DateTime.Now.AddDays(1)
                }
            };
            var user = new User
            {
                Id = request.UserId
            };

            _mockUserRepository.Setup(m => m.Get(It.Is<string>(u => u.Equals(request.Username)))).ReturnsAsync(user);

            _mockReservationRepository.Setup(m => m.Get(It.Is<Guid>(hr => hr.Equals(request.ReservationId)))).ReturnsAsync((Reservation)null);

            var result = await _handler.Handle(request, default);

            Assert.NotNull(result);
            Assert.IsType<NotFoundResponse>(result);
            Assert.NotEmpty(result.Errors);
            Assert.Equal("Reservation not found", result.Errors.FirstOrDefault());

            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Handle_Should_Return_NotFoundResponse_Room_Not_Found()
        {
            var request = new ChangeReservationRequest
            {
                ReservationId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Username = "testuser",
                Dates = new List<DateTime>
                {
                    DateTime.Now.AddDays(1)
                }
            };
            var user = new User
            {
                Id = request.UserId
            };
            var reservation = new Reservation
            {
                HotelRoomId = Guid.NewGuid()
            };

            _mockUserRepository.Setup(m => m.Get(It.Is<string>(u => u.Equals(request.Username)))).ReturnsAsync(user);

            _mockReservationRepository.Setup(m => m.Get(It.Is<Guid>(hr => hr.Equals(request.ReservationId)))).ReturnsAsync(reservation);

            _mockHotelRoomRepository.Setup(m => m.Get(It.Is<Guid>(hr => hr.Equals(reservation.HotelRoomId)))).ReturnsAsync((HotelRoom)null);

            var result = await _handler.Handle(request, default);

            Assert.NotNull(result);
            Assert.IsType<NotFoundResponse>(result);
            Assert.NotEmpty(result.Errors);
            Assert.Equal("Room not found.", result.Errors.FirstOrDefault());

            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Handle_Should_Return_BadRequestResponse_Date_Already_Booked()
        {
            var request = new ChangeReservationRequest
            {
                ReservationId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Username = "testuser",
                Dates = new List<DateTime>
                {
                    DateTime.Now.AddDays(1)
                }
            };
            var user = new User
            {
                Id = request.UserId
            };
            var reservation = new Reservation
            {
                HotelRoomId = Guid.NewGuid()
            };
            var room = new HotelRoom
            {
                Id = Guid.NewGuid(),
                Number = "11",
                Reservations = new List<Reservation>
                {
                    new Reservation
                    {
                        Id = Guid.NewGuid(),
                        ReservationDates = new List<ReservationDate>
                        {
                            new ReservationDate
                            {
                                ReservedOn = DateTime.Now.AddDays(1)
                            }
                        }
                    },
                    reservation
                }
            };

            _mockUserRepository.Setup(m => m.Get(It.Is<string>(u => u.Equals(request.Username)))).ReturnsAsync(user);

            _mockReservationRepository.Setup(m => m.Get(It.Is<Guid>(hr => hr.Equals(request.ReservationId)))).ReturnsAsync(reservation);

            _mockHotelRoomRepository.Setup(m => m.Get(It.Is<Guid>(hr => hr.Equals(reservation.HotelRoomId)))).ReturnsAsync(room);

            var result = await _handler.Handle(request, default);

            Assert.NotNull(result);
            Assert.IsType<BadRequestResponse>(result);
            Assert.NotEmpty(result.Errors);
            Assert.Equal("Date chosen has already been booked", result.Errors.FirstOrDefault());

            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Handle_Should_Return_Success_Response()
        {
            var request = new ChangeReservationRequest
            {
                ReservationId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Username = "testuser",
                Dates = new List<DateTime>
                {
                    DateTime.Now.AddDays(1)
                }
            };
            var user = new User
            {
                Id = request.UserId
            };
            var room = new HotelRoom
            {
                Id = Guid.NewGuid(),
                Number = "11",
                Reservations = new List<Reservation>
                {
                    new Reservation
                    {
                        Id = Guid.NewGuid(),
                        ReservationDates = new List<ReservationDate>
                        {
                            new ReservationDate
                            {
                                ReservedOn = DateTime.Now.AddDays(2)
                            }
                        }
                    }
                }
            };
            var reservation = new Reservation
            {
                User = user,
                HotelRoom = room,
                HotelRoomId = room.Id,
                UserId = user.Id,
                ReservationDates = new List<ReservationDate>
                {
                    new ReservationDate
                    {
                        ReservedOn = DateTime.Now.AddDays(3)
                    }
                }
            };
            room.Reservations.Add(reservation);
            

            _mockUserRepository.Setup(m => m.Get(It.Is<string>(u => u.Equals(request.Username)))).ReturnsAsync(user);

            _mockReservationRepository.Setup(m => m.Get(It.Is<Guid>(hr => hr.Equals(request.ReservationId)))).ReturnsAsync(reservation);

            _mockHotelRoomRepository.Setup(m => m.Get(It.Is<Guid>(hr => hr.Equals(reservation.HotelRoomId)))).ReturnsAsync(room);

            _mockUnitOfWork.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var result = await _handler.Handle(request, default);

            Assert.NotNull(result);
            Assert.IsType<Response>(result);
            Assert.Empty(result.Errors);
            Assert.IsType<ChangeReservationResponse>(result.Message);
            Assert.Equal("Reservation changed succesfully", (result.Message as ChangeReservationResponse).Message);
            Assert.Equal(user.Id, (result.Message as ChangeReservationResponse).UserId);
            Assert.Equal(room.Id, (result.Message as ChangeReservationResponse).HotelRoomId);
            Assert.Equal(request.Dates.Count, (result.Message as ChangeReservationResponse).Dates.Count);
            Assert.Equal(request.Dates.FirstOrDefault(), reservation.ReservationDates.FirstOrDefault().ReservedOn);

            _mockRepository.VerifyAll();
        }
    }
}


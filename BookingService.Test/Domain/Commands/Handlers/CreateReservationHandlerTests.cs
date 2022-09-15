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
    public class CreateReservationHandlerTests
    {
        private readonly MockRepository _mockRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IHotelRoomRepository> _mockHotelRoomRepository;

        private readonly CreateReservationHandler _handler;

        public CreateReservationHandlerTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _mockUnitOfWork = _mockRepository.Create<IUnitOfWork>();
            _mockUserRepository = _mockRepository.Create<IUserRepository>();
            _mockHotelRoomRepository = _mockRepository.Create<IHotelRoomRepository>();

            _handler = new CreateReservationHandler(_mockUnitOfWork.Object, _mockUserRepository.Object, _mockHotelRoomRepository.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_ErrorResponse()
        {
            var request = new CreateReservationRequest
            {
                UserId = Guid.NewGuid(),
                HotelRoomId = Guid.NewGuid(),
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
            var request = new CreateReservationRequest
            {
                UserId = Guid.NewGuid(),
                HotelRoomId = Guid.NewGuid(),
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
            var request = new CreateReservationRequest
            {
                UserId = Guid.NewGuid(),
                HotelRoomId = Guid.NewGuid(),
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
            var request = new CreateReservationRequest
            {
                UserId = Guid.NewGuid(),
                HotelRoomId = Guid.NewGuid(),
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
            var request = new CreateReservationRequest
            {
                UserId = Guid.NewGuid(),
                HotelRoomId = Guid.NewGuid(),
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
            var request = new CreateReservationRequest
            {
                UserId = Guid.NewGuid(),
                HotelRoomId = Guid.NewGuid(),
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
        public async Task Handle_Should_Return_NotFoundResponse_Room_Not_Found()
        {
            var request = new CreateReservationRequest
            {
                UserId = Guid.NewGuid(),
                HotelRoomId = Guid.NewGuid(),
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

            _mockHotelRoomRepository.Setup(m => m.Get(It.Is<Guid>(hr => hr.Equals(request.HotelRoomId)))).ReturnsAsync((HotelRoom)null);

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
            var request = new CreateReservationRequest
            {
                UserId = Guid.NewGuid(),
                HotelRoomId = Guid.NewGuid(),
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
                        ReservationDates = new List<ReservationDate>
                        {
                            new ReservationDate
                            {
                                ReservedOn = DateTime.Now.AddDays(1)
                            }
                        }
                    }
                }
            };

            _mockUserRepository.Setup(m => m.Get(It.Is<string>(u => u.Equals(request.Username)))).ReturnsAsync(user);

            _mockHotelRoomRepository.Setup(m => m.Get(It.Is<Guid>(hr => hr.Equals(request.HotelRoomId)))).ReturnsAsync(room);

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
            var request = new CreateReservationRequest
            {
                UserId = Guid.NewGuid(),
                HotelRoomId = Guid.NewGuid(),
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

            _mockUserRepository.Setup(m => m.Get(It.Is<string>(u => u.Equals(request.Username)))).ReturnsAsync(user);

            _mockHotelRoomRepository.Setup(m => m.Get(It.Is<Guid>(hr => hr.Equals(request.HotelRoomId)))).ReturnsAsync(room);

            _mockUnitOfWork.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var result = await _handler.Handle(request, default);

            Assert.NotNull(result);
            Assert.IsType<Response>(result);
            Assert.Empty(result.Errors);
            Assert.IsType<CreateReservationResponse>(result.Message);
            Assert.Equal("Reservation created succesfully", (result.Message as CreateReservationResponse).Message);
            Assert.Equal(user.Id, (result.Message as CreateReservationResponse).UserId);
            Assert.Equal(room.Id, (result.Message as CreateReservationResponse).HotelRoomId);
            Assert.Equal(request.Dates.Count, (result.Message as CreateReservationResponse).Dates.Count);
            Assert.NotEmpty(user.Reservations);

            _mockRepository.VerifyAll();
        }
    }
}


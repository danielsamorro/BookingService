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
    public class CancelReservationHandlerTests
    {
        private readonly MockRepository _mockRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IReservationRepository> _mockReservationRepository;

        private readonly CancelReservationHandler _handler;

        public CancelReservationHandlerTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _mockUnitOfWork = _mockRepository.Create<IUnitOfWork>();
            _mockUserRepository = _mockRepository.Create<IUserRepository>();
            _mockReservationRepository = _mockRepository.Create<IReservationRepository>();

            _handler = new CancelReservationHandler(_mockUnitOfWork.Object, _mockUserRepository.Object, _mockReservationRepository.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_ErrorResponse()
        {
            var request = new CancelReservationRequest
            {
                ReservationId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Username = "testuser",
            };

            _mockUserRepository.Setup(m => m.Get(It.Is<string>(u => u.Equals(request.Username)))).Throws(new Exception("error"));

            var result = await _handler.Handle(request, default);

            Assert.NotNull(result);
            Assert.IsType<ErrorResponse>(result);
            Assert.NotEmpty(result.Errors);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Handle_Should_Return_NotFoundResponse_User_Not_Found()
        {
            var request = new CancelReservationRequest
            {
                ReservationId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Username = "testuser",
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
            var request = new CancelReservationRequest
            {
                ReservationId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Username = "testuser",
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
            var request = new CancelReservationRequest
            {
                ReservationId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Username = "testuser",
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
        public async Task Handle_Should_Return_Success_Response()
        {
            var request = new CancelReservationRequest
            {
                ReservationId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Username = "testuser",
            };
            var user = new User
            {
                Id = request.UserId
            };
            var reservation = new Reservation
            {
                Id = request.ReservationId,
                User = user,
                UserId = user.Id,
                HotelRoomId = Guid.NewGuid(),
                ReservationDates = new List<ReservationDate>
                {
                    new ReservationDate
                    {
                        ReservedOn = DateTime.Now.AddDays(1)
                    }
                }
            };

            user.Reservations.Add(reservation);

            _mockUserRepository.Setup(m => m.Get(It.Is<string>(u => u.Equals(request.Username)))).ReturnsAsync(user);

            _mockReservationRepository.Setup(m => m.Get(It.Is<Guid>(hr => hr.Equals(request.ReservationId)))).ReturnsAsync(reservation);

            _mockUnitOfWork.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var result = await _handler.Handle(request, default);

            Assert.NotNull(result);
            Assert.IsType<Response>(result);
            Assert.Empty(result.Errors);
            Assert.IsType<CancelReservationResponse>(result.Message);
            Assert.Empty(user.Reservations);

            _mockRepository.VerifyAll();
        }
    }
}

using BookingService.Domain.Commands.Handlers;
using BookingService.Domain.Commands.Requests;
using BookingService.Domain.Commands.Responses;
using BookingService.Domain.Responses;
using BookingService.Infrastructure.Entities;
using BookingService.Infrastructure.Repositories.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace BookingService.Test.Domain.Commands.Handlers
{
    public class GetReservationsHandlerTests
    {
        private readonly MockRepository _mockRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;

        private readonly GetReservationsHandler _signUpHandler;

        public GetReservationsHandlerTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _mockUserRepository = _mockRepository.Create<IUserRepository>();

            _signUpHandler = new GetReservationsHandler(_mockUserRepository.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_NotFoundResponse()
        {
            var request = new GetReservationsRequest
            {
                Username = "testuser",
                UserId = Guid.NewGuid()
            };

            _mockUserRepository
                .Setup(x => x.Get(It.Is<string>(u => u.Equals(request.Username))))
                .ReturnsAsync((User)null);

            var result = await _signUpHandler.Handle(request, default);

            Assert.NotNull(result);
            Assert.IsType<NotFoundResponse>(result);
            Assert.NotEmpty(result.Errors);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Handle_Should_Return_BadRequestResponse()
        {
            var request = new GetReservationsRequest
            {
                Username = "testuser",
                UserId = Guid.NewGuid()
            };

            var user = new User
            {
                Id = Guid.NewGuid()
            };

            _mockUserRepository
                .Setup(x => x.Get(It.Is<string>(u => u.Equals(request.Username))))
                .ReturnsAsync(user);

            var result = await _signUpHandler.Handle(request, default);

            Assert.NotNull(result);
            Assert.IsType<BadRequestResponse>(result);
            Assert.NotEmpty(result.Errors);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Handle_Should_Return_ErrorResponse()
        {
            var request = new GetReservationsRequest
            {
                Username = "testuser",
                UserId = Guid.NewGuid()
            };

            _mockUserRepository
                .Setup(x => x.Get(It.Is<string>(u => u.Equals(request.Username))))
                .Throws(new Exception("error"));

            var result = await _signUpHandler.Handle(request, default);

            Assert.NotNull(result);
            Assert.IsType<ErrorResponse>(result);
            Assert.NotEmpty(result.Errors);

            _mockRepository.VerifyAll();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public async Task Handle_Should_Return_Success_Response(int reservationQuantity)
        {
            var request = new GetReservationsRequest
            {
                Username = "testuser",
                UserId = Guid.NewGuid()
            };

            var user = new User
            {
                Id = request.UserId,
                Reservations = new List<Reservation>()
            };

            for (int i = 0; i < reservationQuantity; i++)
            {
                user.Reservations.Add(new Reservation
                {
                    HotelRoomId = Guid.NewGuid(),
                    Id = Guid.NewGuid(),
                    ReservationDates = new List<ReservationDate>
                    {
                        new ReservationDate
                        {
                            ReservedOn = DateTime.Now
                        }
                    }
                });
            }

            _mockUserRepository
                .Setup(x => x.Get(It.Is<string>(u => u.Equals(request.Username))))
                .ReturnsAsync(user);

            var result = await _signUpHandler.Handle(request, default);

            Assert.NotNull(result);
            Assert.IsType<Response>(result);
            Assert.Empty(result.Errors);
            Assert.IsType<GetReservationsResponse>(result.Message);
            Assert.Equal(user.Reservations.Count, (result.Message as GetReservationsResponse).Reservations.Count);

            _mockRepository.VerifyAll();
        }
    }
}

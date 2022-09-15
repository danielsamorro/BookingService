using BookingService.Domain.Commands.Handlers;
using BookingService.Domain.Commands.Requests;
using BookingService.Domain.Commands.Responses;
using BookingService.Domain.Responses;
using BookingService.Infrastructure.Entities;
using BookingService.Infrastructure.Repositories.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BookingService.Test.Domain.Commands.Handlers
{
    public class GetReservedDatesHandlerTests
    {
        private readonly MockRepository _mockRepository;
        private readonly Mock<IHotelRoomRepository> _mockHotelRoomRepository;

        private readonly GetReservedDatesHandler _handler;

        public GetReservedDatesHandlerTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _mockHotelRoomRepository = _mockRepository.Create<IHotelRoomRepository>();

            _handler = new GetReservedDatesHandler(_mockHotelRoomRepository.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_ErrorResponse()
        {
            var request = new GetReservedDatesRequest
            {
                RoomNumber = "11"
            };

            _mockHotelRoomRepository
                .Setup(x => x.Get(It.Is<string>(rn => rn.Equals(request.RoomNumber))))
                .Throws(new Exception("error"));

            var result = await _handler.Handle(request, default);

            Assert.NotNull(result);
            Assert.IsType<ErrorResponse>(result);
            Assert.NotEmpty(result.Errors);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Handle_Should_Return_NotFoundResponse()
        {
            var request = new GetReservedDatesRequest
            {
                RoomNumber = "11"
            };

            _mockHotelRoomRepository
                .Setup(x => x.Get(It.Is<string>(rn => rn.Equals(request.RoomNumber))))
                .ReturnsAsync((HotelRoom)null);

            var result = await _handler.Handle(request, default);

            Assert.NotNull(result);
            Assert.IsType<NotFoundResponse>(result);
            Assert.NotEmpty(result.Errors);

            _mockRepository.VerifyAll();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public async Task Handle_Should_Return_Success_Response(int reservationQuantity)
        {
            var room = new HotelRoom
            {
                Id = Guid.NewGuid(),
                Number = Guid.NewGuid().ToString(),
                Reservations = new List<Reservation>()
            };

            for (int i = 0; i < reservationQuantity; i++)
            {
                room.Reservations.Add(new Reservation
                {
                    ReservationDates = new List<ReservationDate>
                        {
                            new ReservationDate
                            {
                                ReservedOn = DateTime.Now.AddDays(1)
                            }
                        }
                });
            }

            var request = new GetReservedDatesRequest
            {
                RoomNumber = "11"
            };

            _mockHotelRoomRepository
                .Setup(x => x.Get(It.Is<string>(rn => rn.Equals(request.RoomNumber))))
                .ReturnsAsync(room);

            var result = await _handler.Handle(request, default);

            Assert.NotNull(result);
            Assert.IsType<Response>(result);
            Assert.Empty(result.Errors);
            Assert.IsType<GetReservedDatesResponse>(result.Message);
            Assert.Equal(room.Reservations.Select(r => r.ReservationDates).ToList().Count, (result.Message as GetReservedDatesResponse).ReservedDates.Count);

            _mockRepository.VerifyAll();
        }
    }
}

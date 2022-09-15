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
    public class GetRoomsHandlerTests
    {
        private readonly MockRepository _mockRepository;
        private readonly Mock<IHotelRoomRepository> _mockHotelRoomRepository;

        private readonly GetRoomsHandler _handler;

        public GetRoomsHandlerTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _mockHotelRoomRepository = _mockRepository.Create<IHotelRoomRepository>();

            _handler = new GetRoomsHandler(_mockHotelRoomRepository.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_ErrorResponse()
        {
            var request = new GetRoomsRequest();

            _mockHotelRoomRepository
                .Setup(x => x.GetAll())
                .Throws(new Exception("error"));

            var result = await _handler.Handle(request, default);

            Assert.NotNull(result);
            Assert.IsType<ErrorResponse>(result);
            Assert.NotEmpty(result.Errors);

            _mockRepository.VerifyAll();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public async Task Handle_Should_Return_Success_Response(int roomQuantity)
        {
            var rooms = new List<HotelRoom>();

            for (int i = 0; i < roomQuantity; i++)
            {
                rooms.Add(new HotelRoom
                {
                    Id = Guid.NewGuid(),
                    Number = Guid.NewGuid().ToString()
                });
            }

            var request = new GetRoomsRequest();

            _mockHotelRoomRepository
                .Setup(x => x.GetAll())
                .ReturnsAsync(rooms);

            var result = await _handler.Handle(request, default);

            Assert.NotNull(result);
            Assert.IsType<Response>(result);
            Assert.Empty(result.Errors);
            Assert.IsType<GetRoomsResponse>(result.Message);
            Assert.Equal(rooms.Count, (result.Message as GetRoomsResponse).Rooms.Count);

            rooms.ForEach(x =>
            {
                Assert.Contains((result.Message as GetRoomsResponse).Rooms, hr => hr.Id.Equals(x.Id) && hr.Number.Equals(x.Number));
            });

            _mockRepository.VerifyAll();
        }
    }
}

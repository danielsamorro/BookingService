using BookingService.Api.Controllers;
using BookingService.Domain.Commands.Requests;
using BookingService.Domain.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace BookingService.Test.Api.Controllers
{
    public class HotelRoomControllerTests
    {
        private readonly MockRepository _mockRepository;
        private readonly Mock<IMediator> _mockMediator;

        private readonly HotelRoomController _hotelRoomController;

        public HotelRoomControllerTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _mockMediator = _mockRepository.Create<IMediator>();

            _hotelRoomController = new HotelRoomController(_mockMediator.Object);
        }

        [Fact]
        public async Task GetRooms_Should_Return()
        {
            var response = new Response("test");

            _mockMediator.Setup(m => m.Send(It.IsAny<GetRoomsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);

            var result = await _hotelRoomController.GetRooms();

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task GetReservedDates_Should_Return()
        {
            var roomNumber = "11";

            var response = new Response("test");

            _mockMediator.Setup(m => m.Send(It.Is<GetReservedDatesRequest>(r => r.RoomNumber.Equals(roomNumber)), It.IsAny<CancellationToken>())).ReturnsAsync(response);

            var result = await _hotelRoomController.GetReservedDates(roomNumber);

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);

            _mockRepository.VerifyAll();
        }
    }
}

using BookingService.Api.Controllers;
using BookingService.Domain.Commands.Requests;
using BookingService.Domain.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace BookingService.Test.Api.Controllers
{
    public class ReservationControllerTests
    {
        private readonly MockRepository _mockRepository;
        private readonly Mock<IMediator> _mockMediator;

        private readonly ReservationController _reservationController;

        public ReservationControllerTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _mockMediator = _mockRepository.Create<IMediator>();

            _reservationController = new ReservationController(_mockMediator.Object);

            AddFakeUserIdentity(_reservationController);
        }

        [Fact]
        public async Task GetRooms_Should_Return()
        {
            var userId = Guid.NewGuid();
            var response = new Response("test");

            _mockMediator.Setup(m => m.Send(It.Is<GetReservationsRequest>(r => r.UserId.Equals(userId)), It.IsAny<CancellationToken>())).ReturnsAsync(response);

            var result = await _reservationController.GetReservations(userId);

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task GetReservedDates_Should_Return()
        {
            var request = new CreateReservationRequest
            {
                UserId = Guid.NewGuid(),
                HotelRoomId = Guid.NewGuid(),
                Username = _reservationController.ControllerContext.HttpContext.User.Identity.Name,
                Dates = new List<DateTime> { DateTime.Now.AddDays(1), DateTime.Now.AddDays(2) }
            };
            var response = new Response("test");

            _mockMediator.Setup(m => m.Send(It.Is<CreateReservationRequest>(r => r.Equals(request)), It.IsAny<CancellationToken>())).ReturnsAsync(response);

            var result = await _reservationController.CreateReservation(request);

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task ChangeReservation_Should_Return()
        {
            var request = new ChangeReservationRequest
            {
                UserId = Guid.NewGuid(),
                ReservationId = Guid.NewGuid(),
                Username = _reservationController.ControllerContext.HttpContext.User.Identity.Name,
                Dates = new List<DateTime> { DateTime.Now.AddDays(1), DateTime.Now.AddDays(2) }
            };
            var response = new Response("test");

            _mockMediator.Setup(m => m.Send(It.Is<ChangeReservationRequest>(r => r.Equals(request)), It.IsAny<CancellationToken>())).ReturnsAsync(response);

            var result = await _reservationController.ChangeReservation(request);

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task CancelReservation_Should_Return()
        {
            var request = new CancelReservationRequest
            {
                UserId = Guid.NewGuid(),
                ReservationId = Guid.NewGuid(),
                Username = _reservationController.ControllerContext.HttpContext.User.Identity.Name
            };
            var response = new Response("test");

            _mockMediator.Setup(m => m.Send(It.Is<CancelReservationRequest>(r => r.Equals(request)), It.IsAny<CancellationToken>())).ReturnsAsync(response);

            var result = await _reservationController.CancelReservation(request);

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);

            _mockRepository.VerifyAll();
        }

        private void AddFakeUserIdentity(ReservationController reservationController)
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "testuser")
            }));

            reservationController.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = user
            };
        }
    }
}

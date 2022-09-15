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
    public class AccountControllerTests
    {
        private readonly MockRepository _mockRepository;
        private readonly Mock<IMediator> _mockMediator;

        private readonly AccountController _accountController;

        public AccountControllerTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _mockMediator = _mockRepository.Create<IMediator>();

            _accountController = new AccountController(_mockMediator.Object);
        }

        [Fact]
        public async Task SignUp_Should_Return()
        {
            var request = new SignUpRequest
            {
                EmailAddress = "test@test.com",
                Password = "gmierg893g4ug43",
                UserName = "testuser"
            };
            var response = new Response("test");

            _mockMediator.Setup(m => m.Send(It.Is<SignUpRequest>(r => r.Equals(request)), It.IsAny<CancellationToken>())).ReturnsAsync(response);

            var result = await _accountController.SignUp(request);

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task SignIn_Should_Return()
        {
            var request = new SignInRequest
            {
                Password = "gmierg893g4ug43",
                Username = "testuser"
            };
            var response = new Response("test");

            _mockMediator.Setup(m => m.Send(It.Is<SignInRequest>(r => r.Equals(request)), It.IsAny<CancellationToken>())).ReturnsAsync(response);

            var result = await _accountController.SignIn(request);

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);

            _mockRepository.VerifyAll();
        }
    }
}

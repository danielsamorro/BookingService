using BookingService.Domain.Commands.Handlers;
using BookingService.Domain.Commands.Requests;
using BookingService.Domain.Commands.Responses;
using BookingService.Domain.Responses;
using BookingService.Domain.Services.Interfaces;
using BookingService.Infrastructure.Entities;
using BookingService.Infrastructure.Repositories.Interfaces;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace BookingService.Test.Domain.Commands.Handlers
{
    public class SignInHandlerTests
    {
        private readonly MockRepository _mockRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IAuthTokenService> _mockAuthTokenService;

        private readonly SignInHandler _signInHandler;

        public SignInHandlerTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _mockUserRepository = new Mock<IUserRepository>();
            _mockAuthTokenService = new Mock<IAuthTokenService>();

            _signInHandler = new SignInHandler(_mockUserRepository.Object, _mockAuthTokenService.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_BadRequestResponse()
        {
            var request = new SignInRequest
            {
                Username = "testuser",
                Password = "nifuewubgijgeiw"
            };

            _mockUserRepository
                .Setup(x => x.Get(It.Is<string>(u => u.Equals(request.Username)), It.Is<string>(e => e.Equals(request.Password))))
                .ReturnsAsync((User)null);

            var result = await _signInHandler.Handle(request, default);

            Assert.NotNull(result);
            Assert.IsType<BadRequestResponse>(result);
            Assert.NotEmpty(result.Errors);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Handle_Should_Return_ErrorResponse()
        {
            var request = new SignInRequest
            {
                Username = "testuser",
                Password = "nifuewubgijgeiw"
            };

            _mockUserRepository
                .Setup(x => x.Get(It.Is<string>(u => u.Equals(request.Username)), It.Is<string>(e => e.Equals(request.Password))))
                .Throws(new Exception("error"));

            var result = await _signInHandler.Handle(request, default);

            Assert.NotNull(result);
            Assert.IsType<ErrorResponse>(result);
            Assert.NotEmpty(result.Errors);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Handle_Should_Return_Success_Response()
        {
            var request = new SignInRequest
            {
                Username = "testuser",
                Password = "nifuewubgijgeiw"
            };
            var user = new User
            {
                Id = Guid.NewGuid(),
                EmailAddress = "test@test.com",
                Password = request.Password,
                Username = request.Username
            };
            var token = "nfewiogneirngnegnnerignernioge";

            _mockUserRepository
                .Setup(x => x.Get(It.Is<string>(u => u.Equals(request.Username)), It.Is<string>(e => e.Equals(request.Password))))
                .ReturnsAsync(user);

            _mockAuthTokenService.Setup(m => m.GenerateAuthToken(It.Is<User>(u => u.Equals(user)))).Returns(token);

            var result = await _signInHandler.Handle(request, default);

            Assert.NotNull(result);
            Assert.IsType<Response>(result);
            Assert.Empty(result.Errors);
            Assert.IsType<SignInResponse>(result.Message);
            Assert.Equal(token, (result.Message as SignInResponse).Token);
            Assert.Equal(user.Id, (result.Message as SignInResponse).UserId);
            Assert.Equal(user.EmailAddress, (result.Message as SignInResponse).EmailAddress);
            Assert.Equal(user.Username, (result.Message as SignInResponse).Username);

            _mockRepository.VerifyAll();
        }
    }
}

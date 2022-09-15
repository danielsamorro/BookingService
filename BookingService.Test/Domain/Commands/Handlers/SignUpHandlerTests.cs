using BookingService.Domain.Commands.Handlers;
using BookingService.Domain.Commands.Requests;
using BookingService.Domain.Commands.Responses;
using BookingService.Domain.Responses;
using BookingService.Infrastructure.Entities;
using BookingService.Infrastructure.Repositories.Interfaces;
using BookingService.Infrastructure.SeedWorking.Interfaces;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace BookingService.Test.Domain.Commands.Handlers
{
    public class SignUpHandlerTests
    {
        private readonly MockRepository _mockRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IUserRepository> _mockUserRepository;

        private readonly SignUpHandler _signUpHandler;

        public SignUpHandlerTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _mockUnitOfWork = _mockRepository.Create<IUnitOfWork>();
            _mockUserRepository = _mockRepository.Create<IUserRepository>();

            _signUpHandler = new SignUpHandler(_mockUnitOfWork.Object, _mockUserRepository.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_ErrorResponse()
        {
            var request = new SignUpRequest
            {
                EmailAddress = "test@test.com",
                Username = "testuser",
                Password = "nifuewubgijgeiw"
            };

            _mockUserRepository
                .Setup(x => x.CheckDuplicate(It.Is<string>(u => u.Equals(request.Username)), It.Is<string>(e => e.Equals(request.EmailAddress))))
                .ReturnsAsync(true);

            var result = await _signUpHandler.Handle(request, default);

            Assert.NotNull(result);
            Assert.IsType<ErrorResponse>(result);
            Assert.NotEmpty(result.Errors);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Handle_Should_Return_Success_Response()
        {
            var request = new SignUpRequest
            {
                EmailAddress = "test@test.com",
                Username = "testuser",
                Password = "nifuewubgijgeiw"
            };

            _mockUserRepository
                .Setup(m => m.CheckDuplicate(It.Is<string>(u => u.Equals(request.Username)), It.Is<string>(e => e.Equals(request.EmailAddress))))
                .ReturnsAsync(false);
            _mockUserRepository
                .Setup(m => m.Add(It.Is<User>(
                    u => u.EmailAddress.Equals(request.EmailAddress) && 
                    u.Username.Equals(request.Username) && 
                    u.Password.Equals(request.Password))));

            _mockUnitOfWork.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var result = await _signUpHandler.Handle(request, default);

            Assert.NotNull(result);
            Assert.IsType<Response>(result);
            Assert.Empty(result.Errors);
            Assert.IsType<SignUpResponse>(result.Message);

            _mockRepository.VerifyAll();
        }
    }
}

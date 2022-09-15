using BookingService.Domain.Services;
using BookingService.Infrastructure;
using BookingService.Infrastructure.Entities;
using Microsoft.Extensions.Options;
using Xunit;

namespace BookingService.Test.Domain.Services
{
    public class AuthTokenServiceTests
    {
        private IOptions<BookingServiceSettings> _options;
        private AuthTokenService _tokenService;

        public AuthTokenServiceTests()
        {
            _options = Options.Create<BookingServiceSettings>(new BookingServiceSettings
            {
                AuthSecret = "vnorngierngnreignrengeneriogrreni"
            });

            _tokenService = new AuthTokenService(_options);
        }

        [Fact]
        public void GenerateAuthToken_Should_Return_Token()
        {
            var user = new User
            {
                Username = "testuser",
                Role = "Customer"
            };

            var token = _tokenService.GenerateAuthToken(user);

            Assert.False(string.IsNullOrEmpty(token));
        }

    }
}

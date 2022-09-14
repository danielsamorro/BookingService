using BookingService.Infrastructure.Entities;

namespace BookingService.Api.Services.Interfaces
{
    public interface IAuthTokenService
    {
        string GenerateAuthToken(User user);
    }
}

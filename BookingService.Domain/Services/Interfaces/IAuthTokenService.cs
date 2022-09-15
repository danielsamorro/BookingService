using BookingService.Infrastructure.Entities;

namespace BookingService.Domain.Services.Interfaces
{
    public interface IAuthTokenService
    {
        string GenerateAuthToken(User user);
    }
}

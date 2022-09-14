using BookingService.Domain.Entities;

namespace BookingService.Api.Services.Interfaces
{
    public interface IAuthTokenService
    {
        string GenerateAuthToken(User user);
    }
}

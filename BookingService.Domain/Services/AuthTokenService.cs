using BookingService.Domain.Services.Interfaces;
using BookingService.Infrastructure;
using BookingService.Infrastructure.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookingService.Domain.Services
{
    public class AuthTokenService : IAuthTokenService
    {
        private readonly string _authSecret;
        public AuthTokenService(IOptions<BookingServiceSettings> options)
        {
            _authSecret = options.Value.AuthSecret;
        }

        public string GenerateAuthToken(User user)
        {
            var tokenHanlder = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_authSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHanlder.CreateToken(tokenDescriptor);

            return tokenHanlder.WriteToken(token);
        }
    }
}

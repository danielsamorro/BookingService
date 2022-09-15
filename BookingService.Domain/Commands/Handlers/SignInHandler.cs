using BookingService.Domain.Commands.Requests;
using BookingService.Domain.Commands.Responses;
using BookingService.Domain.Responses;
using BookingService.Domain.Services.Interfaces;
using BookingService.Infrastructure.Repositories.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BookingService.Domain.Commands.Handlers
{
    public class SignInHandler : IRequestHandler<SignInRequest, Response>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthTokenService _authTokenService;

        public SignInHandler(IUserRepository userRepository, IAuthTokenService authTokenService)
        {
            _userRepository = userRepository;
            _authTokenService = authTokenService;
        }

        public async Task<Response> Handle(SignInRequest request, CancellationToken cancellationToken)
        {
            Response response;

            try
            {
                var user = await _userRepository.Get(request.Username, request.Password);

                if (user == null)
                    throw new Exception("Invalid username or password.");

                var token = _authTokenService.GenerateAuthToken(user);

                response = new Response(new SignInResponse
                {
                    UserId = user.Id,
                    EmailAddress = user.EmailAddress,
                    Username = user.Username,
                    Token = token
                });
            }
            catch (Exception ex)
            {
                response = new ErrorResponse(ex.Message);
            }

            return response;
        }
    }
}

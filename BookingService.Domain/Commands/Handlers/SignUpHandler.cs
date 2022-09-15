using BookingService.Domain.Commands.Requests;
using BookingService.Domain.Commands.Responses;
using BookingService.Domain.Responses;
using BookingService.Infrastructure.Entities;
using BookingService.Infrastructure.Repositories.Interfaces;
using BookingService.Infrastructure.SeedWorking.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BookingService.Domain.Commands.Handlers
{
    public class SignUpHandler : IRequestHandler<SignUpRequest, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;

        public SignUpHandler(IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        public async Task<Response> Handle(SignUpRequest request, CancellationToken cancellationToken)
        {
            Response response;

            try
            {
                var userExists = await _userRepository.CheckDuplicate(request.UserName, request.EmailAddress);

                if (userExists)
                    throw new Exception("Could not sign up user.");

                var user = new User
                {
                    EmailAddress = request.EmailAddress,
                    Username = request.UserName,
                    Password = request.Password,
                    Role = "Customer"
                };

                _userRepository.Add(user);

                await _unitOfWork.SaveChangesAsync();

                response = new Response(new SignUpResponse
                {
                    UseId = user.Id,
                    EmailAddress = user.EmailAddress,
                    Username = user.Username
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

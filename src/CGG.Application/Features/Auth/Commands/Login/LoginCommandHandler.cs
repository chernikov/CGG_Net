using AutoMapper;
using CGG.Application.DTOs.Auth;
using CGG.Application.Interfaces;
using MediatR;

namespace CGG.Application.Features.Auth.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public LoginCommandHandler(
            IAuthService authService,
            IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }

        public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // Validate credentials through service
            var (success, user, errorMessage) = await _authService.ValidateUserCredentialsAsync(
                request.Email,
                request.Password,
                cancellationToken);

            if (!success || user == null)
            {
                throw new UnauthorizedAccessException(errorMessage ?? "Invalid credentials");
            }

            // Generate JWT token
            var token = _authService.GenerateJwtToken(user);

            // Map user to DTO
            var userDto = _mapper.Map<UserDto>(user);

            // Return response
            return new LoginResponseDto
            {
                Token = token,
                User = userDto
            };
        }
    }
}

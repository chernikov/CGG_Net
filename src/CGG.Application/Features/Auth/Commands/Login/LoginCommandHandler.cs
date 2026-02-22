using AutoMapper;
using CGG.Application.DTOs.Auth;
using CGG.Application.Interfaces;
using MediatR;

namespace CGG.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
{
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;

    public LoginCommandHandler(IAuthService authService, IMapper mapper)
    {
        _authService = authService;
        _mapper = mapper;
    }

    public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var (success, user, errorMessage) = await _authService.ValidateUserCredentialsAsync(
            request.Email,
            request.Password,
            cancellationToken);

        if (!success || user == null)
            throw new UnauthorizedAccessException(errorMessage ?? "Invalid credentials");

        var availableContexts = await _authService.GetAvailableContextsAsync(user, cancellationToken);

        var activeContext = availableContexts.FirstOrDefault(c => c.Type == ContextType.Family)
            ?? availableContexts.FirstOrDefault(c => c.Type == ContextType.School)
            ?? availableContexts.First(c => c.Type == ContextType.System);

        var token = _authService.GenerateJwtToken(user, activeContext);
        var userDto = _mapper.Map<UserDto>(user);

        return new LoginResponseDto
        {
            Token = token,
            User = userDto,
            ActiveContext = activeContext,
            AvailableContexts = availableContexts
        };
    }
}

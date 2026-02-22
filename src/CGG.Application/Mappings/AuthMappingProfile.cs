using AutoMapper;
using CGG.Application.DTOs.Auth;
using CGG.Application.Features.Auth.Commands.Login;
using CGG.Application.Features.Auth.Commands.Register;
using CGG.Core.Entities;

namespace CGG.Application.Mappings
{
    public class AuthMappingProfile : Profile
    {
        public AuthMappingProfile()
        {
            // User -> UserDto
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email ?? string.Empty));

            // LoginRequestDto -> LoginCommand
            CreateMap<LoginRequestDto, LoginCommand>();

            // RegisterRequestDto -> RegisterCommand
            CreateMap<RegisterRequestDto, RegisterCommand>();
        }
    }
}

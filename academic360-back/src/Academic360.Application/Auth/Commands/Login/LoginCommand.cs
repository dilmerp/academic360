using MediatR;

namespace Academic360.Application.Auth.Commands.Login;

public class LoginCommand : IRequest<LoginResponseDto>
{
    public string Usuario { get; set; } = string.Empty;
    public string Contrasena { get; set; } = string.Empty;
}
namespace Academic360.Application.Auth.Commands.Login;

public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;
    public long IdUsuario { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
}
namespace TalanLunch.Application.Dtos.Auth
{
    public class LoginDto
    {
        public required string EmailAddress { get; set; }
        public required string Password { get; set; }
    }
}

namespace TalanLunch.Application.Dtos.Auth
{
    public class TokenResponseDto
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
        public bool IsApproved { get; set; } = false;

    }
}

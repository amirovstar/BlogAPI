namespace BlogApp.Business.Dtos.UserDtos;

public class TokenResponseDto
{
    public string Token { get; set; }
    public string Username { get; set; }
    public DateTime Expires { get; set; }
}

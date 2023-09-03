using Microsoft.AspNetCore.Identity;

namespace BlogApp.Business.Dtos.UserDtos;

public record UserWithRolesDto
{
    public AuthorDto User { get; set; }
    public IEnumerable<string> Roles { get; set; }
}

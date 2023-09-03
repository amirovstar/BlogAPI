using BlogApp.Business.Dtos.UserDtos;
using BlogApp.Core.Enums;

namespace BlogApp.Business.Dtos.BlogLikeDtos;

public record BlogLikeListItemDto
{
    public AuthorDto AppUser { get; set; }
    public Reactions Reactions { get; set; }

}

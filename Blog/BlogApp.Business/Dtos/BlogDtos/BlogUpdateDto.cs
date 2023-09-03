using FluentValidation;

namespace BlogApp.Business.Dtos.BlogDtos;

public record BlogUpdateDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string? CoverImageUrl { get; set; }
    public IEnumerable<int> CategoryIds { get; set; }

}
public class BlogYpdateDtoValidator : AbstractValidator<BlogUpdateDto>
{
    public BlogYpdateDtoValidator()
    {
        RuleFor(b => b.Title)
           .NotEmpty()
           .NotNull()
           .MaximumLength(256);
        RuleFor(b => b.Description)
            .NotEmpty()
            .NotNull();
        RuleFor(b => b.CategoryIds)
           .Must(b => IsDistinct(b))
           .WithMessage("Idler tekrarlana bilmez");
    }
    private bool IsDistinct(IEnumerable<int> ids)
    {
        var encounteredIds = new HashSet<int>();

        foreach (var id in ids)
        {
            if (!encounteredIds.Contains(id))
            {
                encounteredIds.Add(id);
            }
            else
            {
                return false;
            }
        }

        return true;
    }
}

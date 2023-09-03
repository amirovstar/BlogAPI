using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace BlogApp.Business.Dtos.CategoryDtos;

public record CategoryUpdateDto
{
    public string Name { get; set; }
    public IFormFile? Logo { get; set; }
}
public class CategoryUpdateDtoValidator : AbstractValidator<CategoryUpdateDto>
{
    public CategoryUpdateDtoValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
                .WithMessage("Kateoqriya adi bos ola bilmez")
            .NotNull()
                .WithMessage("Kateoqriya adi null ola bilmez")
            .MaximumLength(64)
                .WithMessage("Kateoqiya adi 64-den cox ola bilmez");
    }
}

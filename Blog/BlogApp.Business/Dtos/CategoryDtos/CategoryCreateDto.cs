using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace BlogApp.Business.Dtos.CategoryDtos;

public record CategoryCreateDto
{
    public string Name { get; set; }
    public IFormFile Logo { get; set; }
}
public class CategoryCreateDtoValidator : AbstractValidator<CategoryCreateDto>
{
    public CategoryCreateDtoValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
                .WithMessage("Kateoqriya adi bos ola bilmez")
            .NotNull()
                .WithMessage("Kateoqriya adi null ola bilmez")
            .MaximumLength(64)
                .WithMessage("Kateoqiya adi 64-den cox ola bilmez");
        RuleFor(c => c.Logo)
            .NotNull()
                .WithMessage("Kateqoriya Fayl adi 64-den cox ola bilmez");
    }
}

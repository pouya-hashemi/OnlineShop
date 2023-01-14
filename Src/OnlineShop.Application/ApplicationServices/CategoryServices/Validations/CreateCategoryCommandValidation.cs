using FluentValidation;
using OnlineShop.Application.ApplicationServices.CategoryServices.Commands;
using OnlineShop.Domain.EntityPropertyConfigurations;

namespace OnlineShop.Application.ApplicationServices.CategoryServices.Validations;

public class CreateCategoryCommandValidation:AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidation()
    {
        RuleFor(r => r.CategoryName)
            .NotNull().WithName("Category Name").WithMessage("{PropertyName} is required")
            .NotEmpty().WithMessage("{PropertyName} is required")
            .MaximumLength(CategoryPropertyConfiguration.NameMaxLength)
            .WithMessage("Maximum length of {PropertyName} is: {MaxLength}");
    }
}
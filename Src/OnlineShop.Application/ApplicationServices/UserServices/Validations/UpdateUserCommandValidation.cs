using FluentValidation;
using OnlineShop.Application.ApplicationServices.UserServices.Commands;
using OnlineShop.Domain.EntityPropertyConfigurations;

namespace OnlineShop.Application.ApplicationServices.UserServices.Validations;

public class UpdateUserCommandValidation:AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidation()
    {
        RuleFor(r => r.Username)
            .NotNull().WithName("Username").WithMessage("{PropertyName} is required")
            .NotEmpty().WithMessage("{PropertyName} is required")
            .MinimumLength(UserPropertyConfiguration.UsernameMinLength)
            .WithMessage("Minimum length of {PropertyName} is: {MinLength}")
            .MaximumLength(UserPropertyConfiguration.UsernameMaxLength)
            .WithMessage("Maximum length of {PropertyName} is: {MaxLength}");
        
        RuleFor(r=>r.UserTitle)
            .NotNull().WithName("Name").WithMessage("{PropertyName} is required")
            .NotEmpty().WithMessage("{PropertyName} is required")
            .MinimumLength(UserPropertyConfiguration.UserTitleMinLength)
            .WithMessage("Minimum length of {PropertyName} is: {MinLength}")
            .MaximumLength(UserPropertyConfiguration.UserTitleMaxLength)
            .WithMessage("Maximum length of {PropertyName} is: {MaxLength}");
    }
}
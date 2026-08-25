using FluentValidation;
using SecureAuth.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecureAuth.Application.Validators
{
    public class RegisterValidator : AbstractValidator<RegisterDto>
    {
        public RegisterValidator() 
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(100).WithMessage("First name cannot exceed 100 characters.");
            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters.");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email address is required.");
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .Matches(@"^[a-zA-Z0-9_.]+$").WithMessage("Username can only contain letters, numbers, dots, and underscores.")
                .MaximumLength(100).WithMessage("Username cannot exceed 100 characters.");


            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required.")

                .MinimumLength(8)
                .WithMessage("Password must be at least 8 characters long.")

                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).+$")
                .WithMessage(
                    "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character."
                );
        }
    }
}

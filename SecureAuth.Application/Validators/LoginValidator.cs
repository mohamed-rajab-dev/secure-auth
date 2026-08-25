using FluentValidation;
using SecureAuth.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecureAuth.Application.Validators
{
    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator() 
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email address is required.");
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

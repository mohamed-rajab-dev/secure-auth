using FluentValidation;
using SecureAuth.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecureAuth.Application.Validators
{
    public class VerifyOtpVaildator : AbstractValidator<VerifyOtpDto>
    {
        public VerifyOtpVaildator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email address.");
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("OTP code is required.")
                .Matches(@"^\d{6}$")
                .Length(6).WithMessage("OTP code must be 6 digits.");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FluentValidation;
using TvShowTracker.Domain.Models;

namespace TvShowTracker.Infrastructure.Validators
{
    public class UserModelValidator : AbstractValidator<UserModel> {
        public UserModelValidator()
        {
            RuleFor(u => u.FirstName).MinimumLength(2).MaximumLength(100);
            RuleFor(u => u.LastName).MinimumLength(2).MaximumLength(100);
            RuleFor(u => u.Password).MinimumLength(8);
            RuleFor(u => u.Email).NotEmpty().Must(r => MailAddress.TryCreate(r, out _));
        }
    }
}

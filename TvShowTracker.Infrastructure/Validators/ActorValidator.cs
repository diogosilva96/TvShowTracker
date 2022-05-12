using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using TvShowTracker.Domain.Models;

namespace TvShowTracker.Infrastructure.Validators
{
    public class ActorValidator : AbstractValidator<ActorModel>
    {
        public ActorValidator()
        {
            RuleFor(a => a.BirthDate).LessThan(DateTime.Now);
            RuleFor(a => a.Description).MaximumLength(500);
            RuleFor(a => a.FirstName).MinimumLength(2).MaximumLength(100);
            RuleFor(a => a.LastName).MinimumLength(2).MaximumLength(100);
        }
    }
}

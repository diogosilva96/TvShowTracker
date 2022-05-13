using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using TvShowTracker.Domain.Models;

namespace TvShowTracker.Infrastructure.Validators
{
    public class TvShowModelValidator : AbstractValidator<TvShowDetailsModel>
    {
        public TvShowModelValidator()
        {
            RuleFor(t => t.Name).MinimumLength(2).MaximumLength(100);
            RuleFor(t => t.Description).MinimumLength(2).MaximumLength(500);
            RuleFor(t => t.Genres).NotEmpty();
        }
    }
}

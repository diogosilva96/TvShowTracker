using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using TvShowTracker.Domain.Models;

namespace TvShowTracker.Infrastructure.Validators
{
    public class TvShowModelValidator : AbstractValidator<TvShowModel>
    {
        public TvShowModelValidator()
        {
            RuleFor(t => t.Title).MinimumLength(2).MaximumLength(100);
            RuleFor(t => t.Synopsis).MinimumLength(2).MaximumLength(500);
            RuleFor(t => t.Genres).NotEmpty();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using TvShowTracker.Domain.Models;

namespace TvShowTracker.Infrastructure.Validators
{
    public class EpisodeDtoValidator : AbstractValidator<EpisodeDto>
    {
        public EpisodeDtoValidator()
        {
            RuleFor(e => e.Title).MinimumLength(2).MaximumLength(100);
            RuleFor(e => e.Synopsis).MinimumLength(2).MaximumLength(250);
        }
    }
}

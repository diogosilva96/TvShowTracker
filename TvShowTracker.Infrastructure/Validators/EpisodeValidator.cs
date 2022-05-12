﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using TvShowTracker.Domain.Models;

namespace TvShowTracker.Infrastructure.Validators
{
    public class EpisodeValidator : AbstractValidator<EpisodeModel>
    {
        public EpisodeValidator()
        {
            RuleFor(e => e.Name).MinimumLength(2).MaximumLength(100);
        }
    }
}

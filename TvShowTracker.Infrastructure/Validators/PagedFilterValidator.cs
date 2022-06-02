using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using TvShowTracker.Domain.Models;

namespace TvShowTracker.Infrastructure.Validators
{
    public class PagedFilterValidator : AbstractValidator<PagedFilter>
    {
        public PagedFilterValidator()
        {
            RuleFor(pf => pf.Page).NotNull().GreaterThanOrEqualTo(0);
            RuleFor(pf => pf.PageSize).NotNull().LessThanOrEqualTo(500);
        }

    }
}

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
            RuleFor(pf => pf)
                .Must(IsPageSizeAndPageBothNullOrNotNull)
                .WithMessage("Page size and page must be either both null or not null.");
        }

        private bool IsPageSizeAndPageBothNullOrNotNull(PagedFilter filter) =>
            (filter.Page is null && filter.PageSize is null) ||
            (filter.PageSize is not null && filter.Page is not null);
    }
}

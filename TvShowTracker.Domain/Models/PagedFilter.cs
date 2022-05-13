using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TvShowTracker.Domain.Models
{
    public class PagedFilter
    {
        public int? Page { get; set; }
        public int? PageSize { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TvShowTracker.Domain.Models
{
    public class GetActorsFilter : PagedFilter
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public DateTime? BirthDate { get; set; }

        public string? Description { get; set; }

        public IEnumerable<TvShowModel>? Shows { get; set; }

        public bool? IsActive { get; set; }

        public GetActorsFilter()
        {
            IsActive = true;
            Page = 0;
            PageSize = 25;
        }
    }
}

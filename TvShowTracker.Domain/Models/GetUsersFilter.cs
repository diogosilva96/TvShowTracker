using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TvShowTracker.Domain.Models
{
    public class GetUsersFilter : PagedFilter
    {
        public bool? IsActive { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public GetUsersFilter()
        {
            IsActive = true;
            Page = 0;
            PageSize = 25;
        }
    }
}

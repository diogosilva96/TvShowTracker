using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TvShowTracker.Domain.Models
{
    public class TvShowModel
    {
        public string Id { get; set; }

        public string? Name { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string? Country { get; set; }

        public string? Network { get; set; }

        public string? Status { get; set; }
    }
}

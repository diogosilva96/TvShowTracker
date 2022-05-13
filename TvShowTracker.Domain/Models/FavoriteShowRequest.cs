using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TvShowTracker.Domain.Models
{
    public class FavoriteShowRequest
    {
        public IEnumerable<string> ShowIds { get; set; }
    }
}

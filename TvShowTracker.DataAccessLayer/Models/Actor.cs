using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TvShowTracker.DataAccessLayer.Models
{
    public class Actor
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string FirstName { get; set; }

        [MaxLength(100)]
        public string LastName { get; set; }

        public DateTime? BirthDate { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public ICollection<TvShow> Shows { get; set; }

        public bool IsActive { get; set; }
    }
}

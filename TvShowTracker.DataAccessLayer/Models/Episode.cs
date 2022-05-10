using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TvShowTracker.DataAccessLayer.Models
{
    public class Episode
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Title { get; set; }
        public int Season { get; set; }
        public int Number { get; set; }

        [MaxLength(250)]
        public string Synopsis { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime? AddedAt { get; set; }

        public DateTime? ReleasedAt { get; set; }

        public TvShow Show { get; set; }

        [ForeignKey(nameof(Show))]
        public int ShowId { get; set; }

        public bool IsActive { get; set; }
    }
}

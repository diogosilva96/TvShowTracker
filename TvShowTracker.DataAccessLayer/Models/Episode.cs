using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TvShowTracker.DataAccessLayer.Models
{
    public class Episode
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }
        public int Season { get; set; }
        public int Number { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime? AddedAt { get; set; }

        public DateTime? AirDate { get; set; }

        public TvShow Show { get; set; }

        [ForeignKey(nameof(Show))]
        public int ShowId { get; set; }

        public bool? IsActive { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TvShowTracker.DataAccessLayer.Models
{
    public class TvShow
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Synopsis { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime? AddedAt { get; set; }

        public DateTime? ReleasedAt { get; set; }

        public ICollection<Episode> Episodes { get; set; }

        public ICollection<Actor> Cast { get; set; }

        public ICollection<Genre> Genres { get; set; }

        public bool? IsActive { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;


namespace TvShowTracker.DataAccessLayer.Models
{
    [Index(nameof(Country))]
    [Index(nameof(Network))]
    [Index(nameof(Status))]
    public class TvShow
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(2000)]
        public string? Description { get; set; }

        [MaxLength(250)]
        public string? Url { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime? AddedAt { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [MaxLength(50)]
        public string Country { get; set; }

        [MaxLength(75)]
        public string Network { get; set; }

        [MaxLength(50)]
        public string Status { get; set; }

        public ICollection<Episode> Episodes { get; set; }

        public ICollection<Actor> Cast { get; set; }

        public ICollection<Genre> Genres { get; set; }

        public ICollection<User> FavoriteUsers { get; set; }

        public bool? IsActive { get; set; }

        public TvShow()
        {
            FavoriteUsers = new List<User>();
            Genres = new List<Genre>();
            Cast = new List<Actor>();
            Episodes = new List<Episode>();
        }
    }
}

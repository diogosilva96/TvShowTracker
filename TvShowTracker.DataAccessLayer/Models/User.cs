﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TvShowTracker.DataAccessLayer.Models
{
    public class User
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string FirstName { get; set; }

        [MaxLength(100)]
        public string LastName { get; set; }

        [MaxLength(100)]
        public string Password { get; set; }

        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime? RegisteredAt { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? LastUpdatedAt { get; set; }

        public ICollection<TvShow> FavoriteShows { get; set; }

        public bool? IsActive { get; set; }
    }
}

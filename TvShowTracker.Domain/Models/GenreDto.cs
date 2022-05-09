﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TvShowTracker.Domain.Models
{
    public class GenreDto
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public IEnumerable<TvShowDto> Shows { get; set; }

        public bool IsActive { get; set; }
    }
}

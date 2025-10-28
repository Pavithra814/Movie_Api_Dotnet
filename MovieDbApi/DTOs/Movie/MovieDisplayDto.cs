using System;
using System.Collections.Generic;

namespace MovieDbApi.DTOs.Movie
{
    public class MovieDisplayDto
    {
        public string Title { get; set; } = null!;
        public string? StoryLine { get; set; }
        public double? AudienceRating { get; set; }
        public int? AudienceCount { get; set; }
        public string? Genres { get; set; }  // Could be comma-separated string
        public DateOnly? ReleaseDate { get; set; }
        public string? ImageUrl { get; set; }
    }
}

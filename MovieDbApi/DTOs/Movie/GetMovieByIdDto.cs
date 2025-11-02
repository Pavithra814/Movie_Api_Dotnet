using System;
using System.Collections.Generic;

namespace MovieDbApi.DTOs.Movie
{
    public class GetMovieByIdDto
    {
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string? ReleaseDate { get; set; }
        public string StoryLine { get; set; }
        public double? AudienceRating { get; set; }
        public int? AudienceCount { get; set; }
        public string Genres { get; set; }

        // New fields
        public int? RuntimeMinutes { get; set; }
        public string? Language { get; set; }
        public string? Director { get; set; }
        public string? LeadActor { get; set; }
        public string? LeadActress { get; set; }
        public string? SupportingActors { get; set; }
        public string? Period { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieDbApi.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public string? ReleaseDate { get; set; }

        public string StoryLine { get; set; }

        public double? AudienceRating { get; set; }

        public int? AudienceCount { get; set; }

        public string Genres { get; set; }

        // New fields
        public int? RuntimeMinutes { get; set; } // Movie duration
        public string? Language { get; set; }
        public string? Director { get; set; }
        public string? LeadActor { get; set; } 
        public string? LeadActress { get; set; } 
        public string? Period { get; set; } // e.g., "80s", "90s"

        // Comma-separated list for simplicity (or use a separate table for normalization)
        public string? SupportingActors { get; set; }

        // Audit fields
        public DateTime? CreatedOn { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; } = "system";
        public DateOnly? UpdatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string? DeletedBy { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}

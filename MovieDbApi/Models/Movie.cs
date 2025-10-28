using System;
using System.ComponentModel.DataAnnotations;

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
        public DateOnly? ReleaseDate { get; set; }

        public string StoryLine { get; set; }

        public double? AudienceRating { get; set; }

        public int? AudienceCount { get; set; }

        public string Genres { get; set; }

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

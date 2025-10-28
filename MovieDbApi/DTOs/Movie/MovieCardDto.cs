namespace MovieDbApi.DTOs.Movie
{
    public class MovieCardDto
    {
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public DateOnly? ReleaseDate { get; set; }
        public string? Language { get; set; }
        public double? AudienceRating { get; set; }
    }
}

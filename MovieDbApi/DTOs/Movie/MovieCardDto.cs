namespace MovieDbApi.DTOs.Movie
{
    public class MovieCardDto
    {
        public string Title { get; set; } = null!;
        public DateOnly? ReleaseDate { get; set; } = null!;  // string in "yyyy-MM-dd" format
        public string? ImageUrl { get; set; }
    }
}

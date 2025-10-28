using System.ComponentModel.DataAnnotations;

namespace MovieApi.Models
{
    public class Favourite
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int MovieId { get; set; }
    }
}

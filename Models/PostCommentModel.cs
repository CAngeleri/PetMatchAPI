using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MomosMatch.Models
{
    public class PostComment
    {
        [Key]
        public int Id { get; set; }
        public string? Comment {get;set;}

        public int PostId { get; set; }
        public  Post? Post { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MomosMatch.Models
{
    public class Like
    {
        [Key]
        public int Id { get; set; }

        public int PetId { get; set; }
        public Pet? Pet { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}

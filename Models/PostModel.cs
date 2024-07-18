using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MomosMatch.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        
        public string? PostDetails { get; set; }

        public int UserId { get; set; }
        public User? Author { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public ICollection<PostComment>? Comments { get; set; }
        public ICollection<PostLike>? Likes { get; set; }
    }
}

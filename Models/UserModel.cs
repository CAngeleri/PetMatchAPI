#pragma warning disable 
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;

namespace MomosMatch.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [MaxLength(255)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [MaxLength(255)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        [MaxLength(255)]
        [UniqueEmail(ErrorMessage = "Email must be unique.")]
        public string? Email { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        [MaxLength(255)]
        public string Password { get; set; }

        [NotMapped]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public List<Like> LikedPets { get; set; } = new List<Like>(); 
        public List<Post> CreatedPost { get; set; } = new List<Post>(); 
        public List<PostLike> LikedPost {get; set;} = new List<PostLike>();
        public List<PostComment> Comments {get; set;} = new List<PostComment>();
    }
    public class UniqueEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return new ValidationResult("Email is required.");
            }

            var dbContext = (MyContext)validationContext.GetService(typeof(MyContext));

            // Check if there is any existing user with the same email
            if (dbContext.User.Any(u => u.Email == value.ToString()))
            {
                return new ValidationResult("Email must be unique.");
            }

            return ValidationResult.Success;
        }
    }
}

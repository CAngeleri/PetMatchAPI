#pragma warning disable


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MomosMatch.Models
{
    public class Pet 
    {
        [Key]
        public int PetId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string? Breed { get; set; }

        public int? Age { get; set; }

        [MaxLength(50)]
        public string? Gender { get; set; }

        [MaxLength(50)]
        public string? Size { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        [MaxLength(255)]
        public string? Health { get; set; }

        [MaxLength(255)]
        public string? VaccinationStatus { get; set; }

        public bool SpayedNeutered { get; set; }

        [MaxLength(255)]
        public string OrganizationName { get; set; }

        [MaxLength(500)]
        public string OrganizationLink { get; set; }

        [MaxLength(500)]
        public string PhotoUrl { get; set; }
        public string Type { get; set; }


        // Foreign Key for User
        public int UserId { get; set; }

        // Navigation property
        public User? Owner { get; set; }

        public List<Like> LikeList { get; set; } = new List<Like>();

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object EventDate, ValidationContext validationContext)
        {
            if (EventDate is DateTime dateTime)
            {
                if (dateTime > DateTime.Now)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("Event date must be in the future!");
                }
            }
            return new ValidationResult("Invalid date.");
        }
    }
}
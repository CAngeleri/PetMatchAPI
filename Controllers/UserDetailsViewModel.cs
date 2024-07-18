namespace MomosMatch.Controllers
{
    internal class UserDetailsViewModel
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int LikeCount { get; set; }
        public object LikedPets { get; set; }
    }
}
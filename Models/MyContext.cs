#pragma warning disable CS8618

using Microsoft.EntityFrameworkCore;

namespace MomosMatch.Models;

public class MyContext : DbContext
{

    public MyContext(DbContextOptions options) : base(options) { }

    public DbSet<User> User { get; set; }
    public DbSet<Pet> Pet { get; set; }
    public DbSet<Like> Like { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<PostLike> PostLikes { get; set; }
    public DbSet<PostComment> PostComments { get; set; }

}

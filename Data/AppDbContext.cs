using Choresbuddy_dotnet.Models;
using Microsoft.EntityFrameworkCore;
using Task = Choresbuddy_dotnet.Models.Task;

namespace Choresbuddy_dotnet.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> users { get; set; }
        public DbSet<Task> tasks { get; set; }
        public DbSet<Reward> rewards { get; set; }
        public DbSet<Cart> carts { get; set; }
        public DbSet<Trophy> trophies { get; set; }
        public DbSet<Leaderboard> leaderboards { get; set; }
        public DbSet<Progress> progresses { get; set; }
        public DbSet<RewardCart> rewardCarts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}

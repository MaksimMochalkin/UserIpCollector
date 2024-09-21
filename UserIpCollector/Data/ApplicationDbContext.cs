namespace UserIpCollector.Data
{
    using Microsoft.EntityFrameworkCore;
    using UserIpCollector.Data.Entities;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<UserIpAddress> UserIpAddresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<UserIpAddress>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<UserIpAddress>()
                .HasOne(u => u.User)
                .WithMany(u => u.IpAddresses)
                .HasForeignKey(u => u.UserId);
        }
    }
}

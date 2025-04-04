using Microsoft.EntityFrameworkCore;
using QRCodeManagement.API.Models;

namespace QRCodeManagement.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Item> Items { get; set; }
        public DbSet<QRCode> QRCodes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Scan> Scans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<QRCode>()
                .HasOne<Item>()
                .WithMany()
                .HasForeignKey(q => q.ItemId);

            modelBuilder.Entity<QRCode>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(q => q.Id);
        }
    }
}
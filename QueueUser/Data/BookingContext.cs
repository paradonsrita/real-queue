using QMS.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace QMS.Data
{
    public class BookingContext : DbContext
    {
        public BookingContext(DbContextOptions<BookingContext> options) : base(options)
        {
        }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
#if DEBUG
            optionsBuilder.LogTo(Console.WriteLine);
#endif
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(u => u.user_id); // การกำหนดคีย์หลักของ User

            modelBuilder.Entity<Booking>()
                .HasKey(b => b.queue_id); // การกำหนดคีย์หลักของ Booking

            base.OnModelCreating(modelBuilder);
        }
    }
}

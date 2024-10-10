using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using ApiIsocare2.Models.Booking;
using ApiIsocare2.Models.Queue;
using ApiIsocare2.Models.UserModel;


namespace ApiIsocare2.Data
{
    public class AppDbContext : DbContext
    {
        
            public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
            {

            }
        

            public DbSet<User> Users { get; set; }
            public DbSet<QueueType> QueueTypes { get; set; }
            public DbSet<QueueStatus> QueueStatuses { get; set; }
            public DbSet<BookingQueue> BookingQueues { get; set; }
            public DbSet<CounterQueue> CounterQueues { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<User>().ToTable("user");
                modelBuilder.Entity<QueueType>().ToTable("queue_type");
                modelBuilder.Entity<QueueStatus>().ToTable("queue_status");
                modelBuilder.Entity<BookingQueue>().ToTable("booking_queue");
                modelBuilder.Entity<CounterQueue>().ToTable("counter_queue");

                // กำหนดความสัมพันธ์ Foreign Key
                modelBuilder.Entity<BookingQueue>()
                    .HasOne(bq => bq.QueueType)
                    .WithMany()
                    .HasForeignKey(bq => bq.queue_type_id);

                modelBuilder.Entity<BookingQueue>()
                    .HasOne(bq => bq.QueueStatus)
                    .WithMany()
                    .HasForeignKey(bq => bq.queue_status_id);

                modelBuilder.Entity<BookingQueue>()
                    .HasOne(bq => bq.User)
                    .WithMany()
                    .HasForeignKey(bq => bq.user_id);

                
                modelBuilder.Entity<CounterQueue>()
                    .HasOne(cq => cq.QueueType)
                    .WithMany()
                    .HasForeignKey(cq => cq.queue_type_id);

                modelBuilder.Entity<CounterQueue>()
                    .HasOne(cq => cq.QueueStatus)
                    .WithMany()
                    .HasForeignKey(cq => cq.queue_status_id);

            
        }

    }
}

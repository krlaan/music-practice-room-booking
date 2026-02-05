using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Room> Rooms { get; set; } = default!;
    public DbSet<Person> People { get; set; } = default!;
    public DbSet<Booking> Bookings { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Room entity
        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.RoomNumber)
                .IsRequired()
                .HasMaxLength(20);
            entity.HasIndex(r => r.RoomNumber)
                .IsUnique();
            entity.Property(r => r.Size)
                .HasConversion<string>(); // enum value
        });

        // Configure Person entity
        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.FullName)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(p => p.UserType)
                .HasConversion<string>();
            entity.Property(p => p.ProgramType)
                .HasConversion<string>();
            entity.Ignore(p => p.NoShowPenaltyHours); // Calculated property
        });

        // Configure Booking entity
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(b => b.Id);
            
            entity.Property(b => b.Status)
                .HasConversion<string>();

            entity.Ignore(b => b.DurationHours); // Calculated property

            // Configure relationships
            entity.HasOne(b => b.Student)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(b => b.Room)
                .WithMany(r => r.Bookings)
                .HasForeignKey(b => b.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            // Create index for common queries
            entity.HasIndex(b => new { b.RoomId, b.StartTime, b.EndTime });
            entity.HasIndex(b => new { b.StudentId, b.StartTime });
        });
    }
}
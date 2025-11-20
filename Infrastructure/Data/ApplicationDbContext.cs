using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Domain.Enums;

namespace RestaurantReservation.Infrastructure.Data;

/// <summary>
/// Entity Framework Core <see cref="DbContext"/> for the application.
/// Manages access to domain entities (User, Address, Restaurant, Table, Reservation)
/// and contains fluent configuration (indexes, relationships, seed data).
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<UserAccount> UsersTable => Set<UserAccount>();
    public DbSet<Address> AddressesTable => Set<Address>();
    public DbSet<Restaurant> RestaurantsTable => Set<Restaurant>();
    public DbSet<Table> TablesTable => Set<Table>();
    public DbSet<Reservation> ReservationsTable => Set<Reservation>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        ConfigureUsers(builder);
        ConfigureAddresses(builder);
        ConfigureRestaurant(builder);
        ConfigureTable(builder);
        ConfigureReservation(builder);
    }

    private static void ConfigureUsers(ModelBuilder builder)
    {
        builder.Entity<UserAccount>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity
                .Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            entity
                .Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(100);

            entity
                .Property(e => e.CreatedAt)
                .IsRequired();

            entity.HasIndex(e => new { e.FirstName, e.LastName });

            entity.HasData(
                new UserAccount
                {
                    UserId = "user-1",
                    FirstName = "Eugène",
                    LastName = "ETOUNDI",
                    CreatedAt = new DateTime(2023, 1, 1),
                }
            );
        });
    }

    private static void ConfigureAddresses(ModelBuilder builder)
    {
        builder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity
                .Property(e => e.Street)
                .IsRequired()
                .HasMaxLength(200);

            entity
                .Property(e => e.ZipCode)
                .IsRequired();

            entity.Property(e => e.City)
                .IsRequired()
                .HasMaxLength(200);

            entity
                .Property(e => e.Country)
                .IsRequired()
                .HasMaxLength(100);

            entity
                .HasOne(a => a.Restaurant)
                .WithOne(r => r.RestaurantAddress)
                .HasForeignKey<Address>(a => a.RestaurantId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.City);

            entity.HasData(
                new Address
                {
                    Id = 1,
                    Street = "76 Luneau",
                    ZipCode = 49000,
                    City = "Angers",
                    Country = "France",
                    RestaurantId = 1
                });
        });
    }

    private static void ConfigureRestaurant(ModelBuilder builder)
    {
        builder.Entity<Restaurant>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity
                .Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity
                .Property(e => e.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);

            entity.HasIndex(r => r.Name);

            entity.HasData(
                new Restaurant
                {
                    Id = 1,
                    Name = "Bistro Central",
                    PhoneNumber = "+1-555-0101",
                    OpeningTime = TimeSpan.FromHours(11),
                    ClosingTime = TimeSpan.FromHours(22),
                }
            );
        });
    }

    private static void ConfigureTable(ModelBuilder builder)
    {
        builder.Entity<Table>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity
                .Property(e => e.TableNumber)
                .IsRequired();

            entity
                .Property(e => e.Capacity)
                .IsRequired();

            entity
                .HasOne(t => t.Restaurant)
                .WithMany(r => r.Tables)
                .HasForeignKey(t => t.RestaurantId)
                .OnDelete(DeleteBehavior.Cascade);

            // Unique table number per restaurant
            entity.HasIndex(t => new { t.RestaurantId, t.TableNumber })
                .IsUnique();

            entity.HasData(
                new Table { Id = 1, TableNumber = 1, Capacity = 4, RestaurantId = 1 },
                new Table { Id = 2, TableNumber = 2, Capacity = 2, RestaurantId = 1 },
                new Table { Id = 3, TableNumber = 3, Capacity = 6, RestaurantId = 1 }
            );
        });
    }
    private static void ConfigureReservation(ModelBuilder builder)
    {
        builder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity
                .Property(e => e.Name)
                .IsRequired();

            entity
                .Property(e => e.ReservationTime)
                .IsRequired();

            entity
                .Property(e => e.NumberOfGuests)
                .IsRequired();

            entity
                .Property(e => e.Status)
                .IsRequired();

            // Relationship: Reservation belongs to Table
            entity
                .HasOne(r => r.Table)
                .WithMany(t => t.Reservations)
                .HasForeignKey(r => r.TableId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relationship: Reservation belongs to User
            entity
                .HasOne(r => r.User)
                .WithMany(u => u.Reservations)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(r => r.ReservationTime);

            entity.HasData(
                new Reservation
                {
                    Id = 1,
                    Name = "Eug Reservation",
                    ReservationTime = new DateTime(2025, 11, 15, 19, 0, 0, DateTimeKind.Utc),
                    NumberOfGuests = 2,
                    Status = ReservationStatus.Confirmed,
                    TableId = 1,
                    UserId = "user-1"
                }
            );
        });
    }
}
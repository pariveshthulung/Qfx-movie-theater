using Microsoft.EntityFrameworkCore;
using QFX.Entity;
using QFX.Manager.Interface;
using QFX.Models;

namespace QFX.data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Audi> Audis { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Language> Languages { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<ReservationSeat> ReservationSeats { get; set; }
    public DbSet<Seat> Seats { get; set; }
    public DbSet<Show> Shows { get; set; }
    public DbSet<ShowSeat> ShowSeats { get; set; }
    public DbSet<MovieGenre> MovieGenres { get; set; }
    public DbSet<ShowDate> ShowDates { get; set; }
    public DbSet<ShowTime> ShowTimes { get; set; }
    public DbSet<TicketNotify> TicketNotifies { get; set; }

    public DbSet<UserLocationPreference> UserLocationPreferences { get; set; }

    
    public override int SaveChanges()
    {
        foreach(var record in ChangeTracker.Entries())
        {
            var entity = record.Entity;
            if(record.State == EntityState.Deleted && entity is ISoftDelete)
            {
                record.State = EntityState.Modified;
                entity.GetType().GetProperty("IsDeleted").SetValue(entity,true);
            }
        }
        return base.SaveChanges();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Movie>().HasQueryFilter(x=>!x.IsDeleted);
        modelBuilder.Entity<Audi>().HasQueryFilter(x=>!x.IsDeleted);
        modelBuilder.Entity<Reservation>().HasQueryFilter(x=>!x.IsDeleted);
        modelBuilder.Entity<Seat>().HasQueryFilter(x=>!x.IsDeleted);
        modelBuilder.Entity<Show>().HasQueryFilter(x=>!x.IsDeleted);

    }
}

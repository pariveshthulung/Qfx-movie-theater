using Microsoft.EntityFrameworkCore;
using QFX.Entity;
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
}

using Microsoft.EntityFrameworkCore;
using QFX.Entity;

namespace QFX.data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {  
    }

    public DbSet<User> Users { get; set; }
}

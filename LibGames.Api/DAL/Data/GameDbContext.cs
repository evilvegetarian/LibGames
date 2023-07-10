using LibGames.Api.DAL.Entities;
using Microsoft.EntityFrameworkCore;


namespace LibGames.Api.DAL.Data;

public class GameDbContext : DbContext
{
    public GameDbContext(DbContextOptions<GameDbContext> dbContext) : base(dbContext)
    {
    }

    public DbSet<Game> Games { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Studio> Studios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}

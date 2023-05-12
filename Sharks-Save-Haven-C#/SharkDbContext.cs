using Microsoft.EntityFrameworkCore;
using Sharks_Save_Haven.Models;

namespace Sharks_Save_Haven;

public class SharkDbContext : DbContext {
    public DbSet<Game> Games { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite("Data Source=games.db");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game>();
    }
}
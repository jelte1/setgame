using backend.Database.Configs;
using backend.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace backend.Database;

public class SetGameDbContext : IdentityDbContext<User>
{
    public DbSet<Entities.Card> Cards { get; set; }
    public DbSet<Entities.Game> Games { get; set; }
    public DbSet<Entities.FoundSet> Sets { get; set; }

    public SetGameDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Add all configurations from the assembly, no need to add each one separately
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Program).Assembly);
    }
}
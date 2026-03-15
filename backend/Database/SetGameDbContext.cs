using backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace backend.Database;

public class SetGameDbContext: DbContext
{
    public DbSet<Entities.Board> Boards { get; set; }
    public DbSet<Entities.Card> Cards { get; set; }
    public DbSet<Entities.Game> Games { get; set; }
    public DbSet<Entities.Set> Sets { get; set; }
    public DbSet<Entities.User> Users { get; set; }
    
    public SetGameDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Board>()
            .HasMany(b => b.Cards)
            .WithMany();
        
        modelBuilder.Entity<Card>().HasData(CreateAllCards());
    }

    private ICollection<Card> CreateAllCards()
    {
        ICollection<Card> cards = new List<Card>();
        int id = 1;
        
        foreach (var shape in Enum.GetValues<Shape>())
        {
            foreach (var color in Enum.GetValues<Color>())
            {
                foreach (var filling in Enum.GetValues<Filling>())
                {
                    foreach (var amount in Enum.GetValues<Amount>())
                    {
                        cards.Add(new Card
                        {
                            Id = id++,
                            Shape = shape,
                            Color = color,
                            Filling = filling,
                            Amount = amount
                        });
                    }
                }
            } 
        }
        return cards;
    }
}
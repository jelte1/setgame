using backend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Database.Configs;

public class FoundSetConfig : IEntityTypeConfiguration<FoundSet>
{
    public void Configure(EntityTypeBuilder<FoundSet> builder)
    {
        builder.HasOne(f => f.Card1)
            .WithMany()
            .HasForeignKey(f => f.Card1Id)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(f => f.Card2)
            .WithMany()
            .HasForeignKey(f => f.Card2Id)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(f => f.Card3)
            .WithMany()
            .HasForeignKey(f => f.Card3Id)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
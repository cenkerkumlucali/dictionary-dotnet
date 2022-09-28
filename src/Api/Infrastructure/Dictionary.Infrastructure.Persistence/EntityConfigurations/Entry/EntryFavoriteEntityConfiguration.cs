using Dictionary.Api.Domain.Models;
using Dictionary.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dictionary.Infrastructure.Persistence.EntityConfigurations.Entry;

public class EntryFavoriteEntityConfiguration : BaseEntityConfiguration<EntryFavorite>
{
    public override void Configure(EntityTypeBuilder<EntryFavorite> builder)
    {
        base.Configure(builder);
        builder.ToTable("entryfavorite", DictionaryContext.DEFAULT_SCHEMA);
        
        builder.HasOne(c => c.Entry)
            .WithMany(c => c.EntryFavorites)
            .HasForeignKey(c => c.EntryId);
        
        builder.HasOne(c => c.CreatedUser)
            .WithMany(c => c.EntryFavorites)
            .HasForeignKey(c => c.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
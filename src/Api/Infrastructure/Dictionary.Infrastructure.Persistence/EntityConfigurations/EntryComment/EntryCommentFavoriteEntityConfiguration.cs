using Dictionary.Api.Domain.Models;
using Dictionary.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dictionary.Infrastructure.Persistence.EntityConfigurations.EntryComment;

public class EntryCommentFavoriteEntityConfiguration : BaseEntityConfiguration<EntryCommentFavorite>
{
    public override void Configure(EntityTypeBuilder<EntryCommentFavorite> builder)
    {
        base.Configure(builder);
        builder.ToTable("entrycommentfavorite", DictionaryContext.DEFAULT_SCHEMA);
        
        builder.HasOne(c => c.EntryComment)
            .WithMany(c => c.EntryFavorites)
            .HasForeignKey(c => c.EntryCommentId);
        
        builder.HasOne(c => c.CreatedUser)
            .WithMany(c => c.EntryCommentFavorites)
            .HasForeignKey(c => c.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
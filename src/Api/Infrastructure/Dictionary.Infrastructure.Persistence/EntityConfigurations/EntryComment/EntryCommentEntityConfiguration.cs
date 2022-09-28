using Dictionary.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dictionary.Infrastructure.Persistence.EntityConfigurations.EntryComment;

public class EntryCommentEntityConfiguration : BaseEntityConfiguration<Api.Domain.Models.EntryComment>
{
    public override void Configure(EntityTypeBuilder<Api.Domain.Models.EntryComment> builder)
    {
        base.Configure(builder);
        builder.ToTable("entrycomment", DictionaryContext.DEFAULT_SCHEMA);
        builder.HasOne(c => c.CreatedByUser)
            .WithMany(c => c.EntryComments)
            .HasForeignKey(c => c.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(c => c.Entry)
            .WithMany(c => c.EntryComments)
            .HasForeignKey(c => c.EntryId);
    }
}
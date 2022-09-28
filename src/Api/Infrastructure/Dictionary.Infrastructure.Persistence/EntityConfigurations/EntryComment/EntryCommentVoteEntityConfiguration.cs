using Dictionary.Api.Domain.Models;
using Dictionary.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dictionary.Infrastructure.Persistence.EntityConfigurations.EntryComment;

public class EntryCommentVoteEntityConfiguration : BaseEntityConfiguration<EntryCommentVote>
{
    public override void Configure(EntityTypeBuilder<EntryCommentVote> builder)
    {
        base.Configure(builder);
        builder.ToTable("entrycommentvote", DictionaryContext.DEFAULT_SCHEMA);
        
        builder.HasOne(c => c.EntryComment)
            .WithMany(c => c.EntryVotes)
            .HasForeignKey(c => c.EntryCommentId);
    }
}
using Dictionary.Api.Domain.Models;
using Dictionary.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dictionary.Infrastructure.Persistence.EntityConfigurations.Entry;

public class EntryVoteEntityConfiguration : BaseEntityConfiguration<EntryVote>
{
    public override void Configure(EntityTypeBuilder<EntryVote> builder)
    {
        base.Configure(builder);
        builder.ToTable("entryvote", DictionaryContext.DEFAULT_SCHEMA);
        
        builder.HasOne(c => c.Entry)
            .WithMany(c => c.EntryVotes)
            .HasForeignKey(c => c.EntryId);
    }
}
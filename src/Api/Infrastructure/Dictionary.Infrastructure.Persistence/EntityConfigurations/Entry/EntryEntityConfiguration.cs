using Dictionary.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dictionary.Infrastructure.Persistence.EntityConfigurations.Entry;

public class EntryEntityConfiguration : BaseEntityConfiguration<Api.Domain.Models.Entry>
{
    public override void Configure(EntityTypeBuilder<Api.Domain.Models.Entry> builder)
    {
        base.Configure(builder);
        builder.ToTable("entry", DictionaryContext.DEFAULT_SCHEMA);
        builder.HasOne(c => c.CreatedBy).
            WithMany(c => c.Entries)
            .HasForeignKey(c => c.CreatedById);
    }
}
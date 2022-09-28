using Dictionary.Api.Domain.Models;
using Dictionary.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dictionary.Infrastructure.Persistence.EntityConfigurations;

public class UserEntityConfiguration:BaseEntityConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);
        builder.ToTable("user", DictionaryContext.DEFAULT_SCHEMA);
    }
}
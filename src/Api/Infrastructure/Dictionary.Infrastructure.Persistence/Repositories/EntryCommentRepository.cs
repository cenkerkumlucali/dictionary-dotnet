using Dictionary.Api.Application.Interfaces.Repositories;
using Dictionary.Api.Domain.Models;
using Dictionary.Infrastructure.Persistence.Context;

namespace Dictionary.Infrastructure.Persistence.Repositories;

public class EntryCommentRepository: GenericRepository<EntryComment>, IEntryCommentRepository
{
    public EntryCommentRepository(DictionaryContext dbContext) : base(dbContext)
    {
    }
}
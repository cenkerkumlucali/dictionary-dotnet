using Dictionary.Api.Application.Interfaces.Repositories;
using Dictionary.Api.Domain.Models;
using Dictionary.Infrastructure.Persistence.Context;

namespace Dictionary.Infrastructure.Persistence.Repositories;

public class EmailConfirmationRepository: GenericRepository<EmailConfirmation>, IEmailConfirmationRepository
{
    public EmailConfirmationRepository(DictionaryContext dbContext) : base(dbContext)
    {
    }
}
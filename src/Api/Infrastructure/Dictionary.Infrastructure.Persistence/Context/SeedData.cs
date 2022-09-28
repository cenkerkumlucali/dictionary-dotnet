using Bogus;
using Common.Infrastructure;
using Dictionary.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Dictionary.Infrastructure.Persistence.Context;

internal class SeedData
{
    private static List<User> GetUsers()
    {
        var result = new Faker<User>("tr")
            .RuleFor(c => c.Id, c => Guid.NewGuid())
            .RuleFor(c => c.CreateDate, c => c.Date.Between(DateTime.Now.AddDays(-100), DateTime.Now))
            .RuleFor(c => c.FirstName, c => c.Person.FirstName)
            .RuleFor(c => c.LastName, c => c.Person.LastName)
            .RuleFor(c => c.Email, c => c.Internet.Email())
            .RuleFor(c => c.UserName, c => c.Internet.UserName())
            .RuleFor(c => c.Password, c => PasswordEncryptor.Encrpt(c.Internet.Password()))
            .RuleFor(c => c.EmailConfirmed, c => c.PickRandom(true, false))
            .Generate(500);

        return result;
    }

    public async Task SeedAsync(IConfiguration configuration)
    {
        var dbContextBuilder = new DbContextOptionsBuilder();
        dbContextBuilder.UseSqlServer(configuration["DictionaryConnectionString"]);
        var context = new DictionaryContext(dbContextBuilder.Options);
        if (context.Users.Any())
        {
            await Task.CompletedTask;
            return;
        }
        var users = GetUsers();
        var userIds = users.Select(c => c.Id);
        await context.Users.AddRangeAsync(users);
        
        var guids = Enumerable.Range(0, 150).Select(c => Guid.NewGuid()).ToList();
        int counter = 0;
        var entries = new Faker<Entry>("tr")
            .RuleFor(c => c.Id, c=>guids[counter++])
            .RuleFor(c => c.CreateDate, c => c.Date.Between(DateTime.Now.AddDays(-100), DateTime.Now))
            .RuleFor(c => c.Subject, c => c.Lorem.Sentence(5, 5))
            .RuleFor(c => c.Content, c => c.Lorem.Paragraph(2))
            .RuleFor(c => c.CreatedById, c => c.PickRandom(userIds))
            .Generate(150);
        await context.Entries.AddRangeAsync(entries);

        var comment = new Faker<EntryComment>("tr")
            .RuleFor(c => c.Id, c=>Guid.NewGuid())
            .RuleFor(c => c.CreateDate, c => c.Date.Between(DateTime.Now.AddDays(-100), DateTime.Now))
            .RuleFor(c => c.Content, c => c.Lorem.Paragraph(2))
            .RuleFor(c => c.CreatedById, c => c.PickRandom(userIds))
            .RuleFor(c => c.EntryId, c => c.PickRandom(guids))
            .Generate(1000);
        await context.EntryComments.AddRangeAsync(comment);
        await context.SaveChangesAsync();
    }
}
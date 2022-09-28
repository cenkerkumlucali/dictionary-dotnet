namespace Dictionary.Api.Domain.Models;

public class EntryComment:BaseEntity
{
    public string Content { get; set; }
    public Guid CreatedById { get; set; }
    public Guid EntryId { get; set; }
    public virtual Entry Entry { get; set; }
    public virtual User CreatedByUser { get; set; }
    public virtual ICollection<EntryCommentVote> EntryVotes { get; set; }
    public virtual ICollection<EntryCommentFavorite> EntryFavorites { get; set; }
}
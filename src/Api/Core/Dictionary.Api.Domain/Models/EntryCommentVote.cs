using Common.Models;

namespace Dictionary.Api.Domain.Models;

public class EntryCommentVote:BaseEntity
{
    public Guid EntryCommentId { get; set; }
    public VoteType VoteType { get; set; }
    public Guid CreatedById { get; set; }
    public virtual EntryComment EntryComment { get; set; }
    // public virtual User CreatedBy { get; set; }
}
namespace Common.Events.EntryComment;

public class DeleteEntryCommentVoteEvent
{
    public Guid EntryCommentId { get; set; }
    public Guid CreatedBy { get; set; }
    
}
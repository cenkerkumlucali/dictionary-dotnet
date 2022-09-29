namespace Common.Events.EntryComment;

public class DeleteEntryCommentFavEvent
{
    public Guid EntryCommentId { get; set; }
    public Guid CreatedBy { get; set; }
}
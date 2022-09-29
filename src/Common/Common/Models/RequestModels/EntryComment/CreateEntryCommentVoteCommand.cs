using MediatR;

namespace Common.Models.RequestModels.EntryComment;

public class CreateEntryCommentVoteCommand:IRequest<bool>
{
    public Guid EntryCommentId { get; set; }
    public VoteType VoteType { get; set; }
    public Guid CreatedBy { get; set; }

    public CreateEntryCommentVoteCommand()
    {
        
    }

    public CreateEntryCommentVoteCommand(Guid entryCommentId, VoteType voteType, Guid createdBy)
    {
        EntryCommentId = entryCommentId;
        VoteType = voteType;
        CreatedBy = createdBy;
    }
}
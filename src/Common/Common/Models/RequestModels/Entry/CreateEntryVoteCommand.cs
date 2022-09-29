using MediatR;

namespace Common.Models.RequestModels.Entry;

public class CreateEntryVoteCommand:IRequest<bool>
{
    public Guid EntryId { get; set; }
    public VoteType VoteType { get; set; }
    public Guid CreatedBy { get; set; }

    public CreateEntryVoteCommand()
    {
        
    }
    public CreateEntryVoteCommand(Guid entryId, VoteType voteType, Guid createdBy)
    {
        EntryId = entryId;
        VoteType = voteType;
        CreatedBy = createdBy;
    }
}
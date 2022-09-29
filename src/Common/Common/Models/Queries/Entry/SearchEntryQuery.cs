using MediatR;

namespace Common.Models.Queries.Entry;

public class SearchEntryQuery:IRequest<List<SearchEntryViewModel>>
{
    public string SearchText { get; set; }

    public SearchEntryQuery()
    {
        
    }
    public SearchEntryQuery(string searchText)
    {
        SearchText = searchText;
    }
}
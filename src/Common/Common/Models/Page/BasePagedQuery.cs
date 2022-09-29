namespace Common.Models.Page;

public class BasePagedQuery
{
    public int Page { get; set; }
    public int PageSize { get; set; }

    public BasePagedQuery(int page, int pageSize)
    {
        Page = page;
        PageSize = pageSize;
    }
}
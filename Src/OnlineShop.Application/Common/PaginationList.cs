namespace OnlineShop.Application.Common;

public class PaginationList<TDto>
{
    public IEnumerable<TDto> List { get;private set; }
    public int CurrentPageIndex { get; private set; }
    public int CurrentPageSize { get; private set; }
    public long TotalCount { get; private set; }
    public bool HasNextPage { get;private set; }
    public bool HasPreviousPage { get;private set; }

    public PaginationList(List<TDto> list,long totalCount,PaginationRequest paginatedRequest)
    {
        List = list;
        CurrentPageIndex = paginatedRequest.PageIndex;
        CurrentPageSize = paginatedRequest.PageSize;
        TotalCount = totalCount;
        HasNextPage = (paginatedRequest.PageIndex * paginatedRequest.PageSize) < totalCount;
        HasPreviousPage = paginatedRequest.PageIndex > 1;
        
    }
}
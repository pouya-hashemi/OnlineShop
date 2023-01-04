using OnlineShop.Application.Common;

namespace OnlineShop.Application.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> AddPaginate<T>(this IQueryable<T> queryable, PaginationRequest paginationRequest)
    {
        return queryable
            .Skip((paginationRequest.PageIndex - 1) * paginationRequest.PageSize)
            .Take(paginationRequest.PageSize)
            .AsQueryable();
    }
}
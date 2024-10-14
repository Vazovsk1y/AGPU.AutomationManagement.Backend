using AGPU.AutomationManagement.Application.Common;

namespace AGPU.AutomationManagement.Application.Extensions;

public static class CommonEx
{
    internal static IQueryable<T> ApplyPaging<T>(this IQueryable<T> source, PagingOptions? pagingOptions)
    {
        if (pagingOptions is null)
        {
            return source;
        }
        
        if (pagingOptions.PageIndex <= 0 || pagingOptions.PageSize <= 0)
        {
            throw new InvalidOperationException();
        }

        return source
            .Skip((pagingOptions.PageIndex - 1) * pagingOptions.PageSize)
            .Take(pagingOptions.PageSize);
    }
}
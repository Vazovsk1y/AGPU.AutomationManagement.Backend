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
    
    /// <returns>null - if value equal to null or whitespace, otherwise trimmed value.</returns>
    public static string? TrimIfNotNullOrWhiteSpace(this string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

    public static async Task<Domain.Entities.User> GetRequiredCurrentUserAsync(this ICurrentUserProvider currentUserProvider)
    {
        var currentUser = await currentUserProvider.GetCurrentUserAsync();
        if (currentUser is null)
        {
            throw new ApplicationException("Current user is null.");
        }
        
        return currentUser;
    }
}
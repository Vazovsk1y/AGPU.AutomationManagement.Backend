namespace AGPU.AutomationManagement.WebApi.Responses;

public class PageResponse<T>
{
    public required IEnumerable<T> Items { get; init; }

    public required int PageIndex { get; init; }

    public required int TotalItemsCount { get; init; }

    public required int TotalPagesCount { get; init; }

    public required bool HasNextPage { get; init; }

    public required bool HasPreviousPage { get; init; }
}
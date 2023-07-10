using Microsoft.EntityFrameworkCore;


namespace LibGames.Api.BL.Pagination;

public class PagedList<T> : List<T>
{
    public int TotalCount { get; }
    public int PageSize { get; }
    public int CurrentPage { get; }
    public int TotalPages { get; }

    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;

    public PagedList(List<T> items, int count, int pageNumber, int pageSize)
    {
        TotalCount = count;
        PageSize = pageSize;
        CurrentPage = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        AddRange(items);
    }

    public static async Task<PagedList<T>> CreateAsync(IQueryable<T> query, int pageNumber, int pageSize, CancellationToken ct)
    {
        var count = await query.CountAsync(ct);

        var items = await query.Skip((pageNumber - 1) * pageSize)
                             .Take(pageSize)
                             .ToListAsync(ct);

        return new PagedList<T>(items, count, pageNumber, pageSize);
    }
}

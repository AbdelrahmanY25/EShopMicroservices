namespace BuildingBlocks.Pagination;

public class PaginatedResult<T>(List<T> items, int count, int pageNumber, int pageSize)
{
	public List<T> Items { get; } = items;
	public int PageNumber { get; } = pageNumber;
	public int TotalPages { get; } = (int)Math.Ceiling(count / (double)pageSize);
	public bool HasPreviousPage => PageNumber > 1;
	public bool HasNextPage => PageNumber < TotalPages;

	public static async Task<PaginatedResult<T>> CreateAsync(IQueryable<T> source, int pageSize, int currentPageNumber)
	{
		int count = source.Count();

		var items = source.Skip((currentPageNumber - 1) * pageSize).Take(pageSize).ToList();

		return new(items, pageSize, currentPageNumber, count);
	}
}
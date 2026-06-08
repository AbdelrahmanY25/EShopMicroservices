namespace Catalog.Api.Products.GetProducts;

public record GetProductsQuery(int? PageNumber = 1, int? PageSize = 10) : IQuery<GetProductsResult>;
public record GetProductsResult(IEnumerable<Product> Products);

public class GetProductsQueryHandler(IDocumentSession session) : IQueryHandler<GetProductsQuery, GetProductsResult>
{
	public async Task<Result<GetProductsResult>> Handle(GetProductsQuery query, CancellationToken cancellationToken)
	{
		var pagedProducts = await session.Query<Product>()
			.ToPagedListAsync(query.PageNumber ?? 1, query.PageSize ?? 10, cancellationToken);

		return Result.Success(new GetProductsResult(pagedProducts));
	}
}
namespace Catalog.Api.Products.GetProductByCategory;

public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryResult>;

public record GetProductByCategoryResult(IEnumerable<Product> Products);

public class GetProductByCategoryQueryHandler(IDocumentSession session) : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
{
	public async Task<Result<GetProductByCategoryResult>> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
	{
		var products = await session.Query<Product>()
			.Where(p => p.Category.Contains(query.Category))
			.ToListAsync(cancellationToken);

		return Result.Success(new GetProductByCategoryResult(products));
	}
}
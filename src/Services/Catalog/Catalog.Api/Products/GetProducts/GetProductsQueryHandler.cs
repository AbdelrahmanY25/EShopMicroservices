namespace Catalog.Api.Products.GetProducts;

public record GetProductsQuery : IQuery<GetProductsResult>;
public record GetProductsResult(IEnumerable<Product> Products);

public class GetProductsQueryHandler(IDocumentSession session) : IQueryHandler<GetProductsQuery, GetProductsResult>
{
	public async Task<Result<GetProductsResult>> Handle(GetProductsQuery query, CancellationToken cancellationToken)
	{
		var products = await session.Query<Product>().ToListAsync(cancellationToken);

		return Result.Success(new GetProductsResult(products));
	}
}

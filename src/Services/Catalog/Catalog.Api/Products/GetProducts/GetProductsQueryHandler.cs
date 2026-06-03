namespace Catalog.Api.Products.GetProducts;

public record GetProductsQuery : IQuery<GetProductsResult>;
public record GetProductsResult(IEnumerable<Product> Products);

public class GetProductsQueryHandler(IDocumentSession session, ILogger<GetProductsQueryHandler> logger) : IQueryHandler<GetProductsQuery, GetProductsResult>
{
	public async Task<Result<GetProductsResult>> Handle(GetProductsQuery query, CancellationToken cancellationToken)
	{
		logger.LogInformation("GetProductsQueryHandler.Handle called with {Query}", query);

		var products = await session.Query<Product>().ToListAsync(cancellationToken);

		return Result.Success(new GetProductsResult(products));
	}
}

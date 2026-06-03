namespace Catalog.Api.Products.GetProductByCategory;

public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryResult>;

public record GetProductByCategoryResult(IEnumerable<Product> Products);

public class GetProductByCategoryQueryHandler(IDocumentSession session, ILogger<GetProductByCategoryQueryHandler> logger)
	: IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
{
	public async Task<Result<GetProductByCategoryResult>> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
	{
		logger.LogInformation("Handling GetProductByCategoryQuery for category: {Category}", query.Category);

		var products = await session.Query<Product>()
			.Where(p => p.Category.Contains(query.Category))
			.ToListAsync(cancellationToken);

		return Result.Success(new GetProductByCategoryResult(products));
	}
}
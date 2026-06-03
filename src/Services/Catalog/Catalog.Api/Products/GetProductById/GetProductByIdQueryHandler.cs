namespace Catalog.Api.Products.GetProductById;

public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;

public record GetProductByIdResult(Product Product);

public class GetProductByIdQueryHandler(IDocumentSession session, ILogger<GetProductByIdQueryHandler> logger) : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
	public async Task<Result<GetProductByIdResult>> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
	{
		logger.LogInformation("GetProductByIdQueryHandler.Handle called with {Query}", query);

		var product = await session.LoadAsync<Product>(query.Id, cancellationToken);

		if (product is null)
			return Result.Failure<GetProductByIdResult>(ProductErrors.ProductNotFound);

		return Result.Success(new GetProductByIdResult(product));
	}
}
namespace Catalog.Api.Products.GetProducts;

public record GetProductsQuery(int? PageNumber = 1, int? PageSize = 10) : IQuery<GetProductsResult>;

public record GetProductsResult(IEnumerable<Product> Products);

public class GetProductRequestQueryValidator : AbstractValidator<GetProductsQuery>
{
	public GetProductRequestQueryValidator()
	{
		RuleFor(x => x.PageNumber)
			.GreaterThan(0)
			.WithMessage("Page number must be greater than 0.");

		RuleFor(x => x.PageSize)
			.GreaterThan(0)
			.LessThanOrEqualTo(50)
			.WithMessage("Page size must be less than or equal to 50.");
	}
}

public class GetProductsQueryHandler(IDocumentSession session) : IQueryHandler<GetProductsQuery, GetProductsResult>
{
	public async Task<Result<GetProductsResult>> Handle(GetProductsQuery query, CancellationToken cancellationToken)
	{
		var pagedProducts = await session.Query<Product>()
			.ToPagedListAsync(query.PageNumber ?? 1, query.PageSize ?? 10, cancellationToken);

		return Result.Success(new GetProductsResult(pagedProducts));
	}
}
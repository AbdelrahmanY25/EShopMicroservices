namespace Catalog.Api.Products.GetProductByCategory;

public record GetProductByCategoryResponse(IEnumerable<Product> Products);

public class GetProductByCategoryEndPoint : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		app.MapGet("/products/category/{category}", async (string category, ISender sender) =>
		{
			var result = await sender.Send(new GetProductByCategoryQuery(category));

			if (result.IsFailure)
				return result.ToProblem();

			var response = result.Value.Adapt<GetProductByCategoryResponse>();
			
			return Results.Ok(response);
		})
		.WithName("GetProductsByCategory")
		.Produces<GetProductByCategoryResponse>(StatusCodes.Status200OK)
		.ProducesProblem(StatusCodes.Status400BadRequest)
		.WithSummary("Gets all products by category")
		.WithDescription("Gets all products by category.");
	}
}

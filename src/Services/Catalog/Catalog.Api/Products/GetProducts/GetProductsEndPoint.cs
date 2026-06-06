namespace Catalog.Api.Products.GetProducts;

public record GetProductResponse(IEnumerable<Product> Products);

public class GetProductsEndPoint : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		app.MapGet("/products", async (ISender sender) =>
		{
			var result = await sender.Send(new GetProductsQuery());

			if (result.IsFailure)
				return result.ToProblem();

			var response = result.Value.Adapt<GetProductResponse>();

			return Results.Ok(response);
		})
		.WithName("GetProducts")
		.Produces<GetProductResponse>(StatusCodes.Status200OK)
		.ProducesProblem(StatusCodes.Status400BadRequest)
		.WithSummary("Gets all products")
		.WithDescription("Gets all products.");
	}
}

namespace Catalog.Api.Products.GetProductById;

public record GetProductByIdResponse(Product Product);

public class GetProductByIdEndPoint() : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		app.MapGet("/products/{id:guid}", async (ISender sender, Guid id) =>
		{
			var result = await sender.Send(new GetProductByIdQuery(id));

			if (result.IsFailure)
				return (IResult)result.ToProblem();

			var response = result.Value.Adapt<GetProductByIdResponse>();

			return Results.Ok(response);
		})
		.WithName("GetProduct")
		.Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
		.ProducesProblem(StatusCodes.Status400BadRequest)
		.WithSummary("Gets product by id")
		.WithDescription("Gets product by id.");
	}
}

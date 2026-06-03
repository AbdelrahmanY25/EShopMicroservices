namespace Catalog.Api.Products.UpdateProduct;

public record UpdateProductRequest(Guid Id, string Name, string Description, decimal Price, List<string> Category, string ImageFile);

public record UpdateProductResponse(bool IsSuccess);

public class UpdateProductEndPoint : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		app.MapPut("/products/", async (UpdateProductRequest request, ISender sender) =>
		{
			var command = request.Adapt<UpdateProductCommand>();
			
			var result = await sender.Send(command);

			if (result.IsFailure)
				return (IResult)result.ToProblem();

			var response = result.Value.Adapt<UpdateProductResponse>();
			
			return Results.Ok(response);
		})
		.WithName("UpdateProduct")
		.Produces<UpdateProductResponse>(StatusCodes.Status200OK)
		.ProducesProblem(StatusCodes.Status400BadRequest)
		.ProducesProblem(StatusCodes.Status404NotFound)
		.WithSummary("Update product")
		.WithDescription("Update product.");
	}
}

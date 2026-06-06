namespace Catalog.Api.Products.DeleteProduct;

public record DeleteProductResponse(bool IsSuccess);

public class DeleteProductEndPoint : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		app.MapDelete("/products/{id:guid}", async (Guid id, ISender sender) =>
		{
			var result = await sender.Send(new DeleteProductCommand(id));

			if (result.IsFailure)
				return result.ToProblem();

			var response = result.Value.Adapt<DeleteProductResponse>();

			return Results.Ok(response);
		})
		.WithName("DeleteProduct")
		.Produces<DeleteProductResult>(StatusCodes.Status200OK)
		.ProducesProblem(StatusCodes.Status400BadRequest)
		.ProducesProblem(StatusCodes.Status404NotFound)
		.WithSummary("Deletes a product.")
		.WithDescription("Deletes a product.");
	}
}

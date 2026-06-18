namespace Basket.Api.Basket.DeleteBasket;

public record DeleteBasketResponse(bool IsSuccess);

public class DeleteBasketEndpoints : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		app.MapDelete("/basket/{userName}", async (string userName, ISender sender) =>
		{
			var result = await sender.Send(new DeleteBasketCommand(userName));

			if (result.IsFailure)
				return result.ToProblem();

			var response = result.Adapt<DeleteBasketResponse>();

			return Results.Ok(response);
		})
		.WithName("DeleteBasket")
		.Produces<DeleteBasketResponse>(StatusCodes.Status200OK)
		.ProducesProblem(StatusCodes.Status400BadRequest)
		.WithSummary("Delete the shopping cart for a specific user.")
		.WithDescription("Delete the shopping cart for a specific user.");
	}
}
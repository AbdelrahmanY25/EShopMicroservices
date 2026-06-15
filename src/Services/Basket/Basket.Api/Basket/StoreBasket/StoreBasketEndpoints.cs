namespace Basket.Api.Basket.StoreBasket;

public record StoreBasketRequest(ShoppingCart Cart);

public record StoreBasketResponse(string UserName);

public class StoreBasketEndpoints : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		app.MapPost("/basket", async (StoreBasketRequest request, ISender sender) =>
		{
			var command = request.Adapt<StoreBasketCommand>();

			var result = await sender.Send(command);

			if (result.IsFailure)
				result.ToProblem();

			var response = result.Value.Adapt<StoreBasketResponse>();

			return Results.Created($"/basket/{response.UserName}", response);
		})
		.WithName("StoreBasket")
		.Produces<StoreBasketResponse>(StatusCodes.Status201Created)
		.ProducesProblem(StatusCodes.Status400BadRequest)
		.WithSummary("Stores the shopping cart for a user.")
		.WithDescription("Stores the shopping cart for a user.");

	}
}

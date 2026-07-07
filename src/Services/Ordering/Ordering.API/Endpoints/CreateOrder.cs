namespace Ordering.API.Endpoints;

public record CreateOrderRequest(OrderDto Order);
public record CreateOrderResponse(Guid Id);

public class CreateOrder : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		app.MapPost("/orders", async (CreateOrderRequest requst, ISender sender) =>
		{
			var command = requst.Adapt<CreateOrderCommand>();

			var result = await sender.Send(command);

			if (result.IsFailure)
				return result.ToProblem();

			var response = result.Value.Adapt<CreateOrderResponse>();

			return Results.Created($"/orders/{response.Id}", response);
		})
		.WithName("CreateOrder")
		.Produces<CreateOrderResponse>(StatusCodes.Status201Created)
		.ProducesProblem(StatusCodes.Status400BadRequest)
		.WithSummary("Creates a new order")
		.WithDescription("Creates a new order.");
	}
}
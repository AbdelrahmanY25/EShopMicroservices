namespace Ordering.API.Endpoints;

public record UpdateOrderRequest(OrderDto Order);
public record UpdateOrderResponse(bool IsSuccess);

public class UpdateOrder : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		app.MapPut("/orders", async (UpdateOrderRequest request, ISender sender) =>
		{
			var command = request.Adapt<UpdateOrderCommand>();

			var result = await sender.Send(command);

			if (result.IsFailure)
				return result.ToProblem();

			var response = result.Value.Adapt<UpdateOrderResponse>();

			return Results.Ok(response);
		})
		.WithName("UpdateOrder")
		.Produces<UpdateOrderResponse>(StatusCodes.Status200OK)
		.ProducesProblem(StatusCodes.Status400BadRequest)
		.WithSummary("Update Order")
		.WithDescription("Update Order");
	}
}
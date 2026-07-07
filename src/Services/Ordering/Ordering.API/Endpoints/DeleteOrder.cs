namespace Ordering.API.Endpoints;

public record DeleteOrderRequest(Guid OrderId);
public record DeleteOrderResponse(bool IsSuccess);

public class DeleteOrder : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		app.MapDelete("/orders/{id}", async (Guid id, ISender sender) =>
		{
			var result = await sender.Send(new DeleteOrderCommand(id));
			
			if (result.IsFailure)
				return result.ToProblem();
			
			var response = result.Value.Adapt<DeleteOrderResponse>();

			return Results.Ok(response);
		})
		.WithName("DeleteOrder")
		.Produces<DeleteOrderResponse>(StatusCodes.Status200OK)
		.ProducesProblem(StatusCodes.Status400BadRequest)
		.ProducesProblem(StatusCodes.Status404NotFound)
		.WithSummary("Delete Order")
		.WithDescription("Delete Order");
	}
}
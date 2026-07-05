namespace Ordering.Application.Orders.Commands.DeleteOrder;

public class DeleteOrderHandler(IAppDbContext dbContext) : ICommandHandler<DeleteOrderCommand, DeleteOrderResult>
{
	public async Task<Result<DeleteOrderResult>> Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
	{
		var orderId = OrderId.Of(command.OrderId);

		var order = await dbContext.Orders.FindAsync([orderId], cancellationToken);

		if (order is null)
			return Result.Failure<DeleteOrderResult>(OrderErrors.OrderNotFound);

		dbContext.Orders.Remove(order);
		await dbContext.SaveChangesAsync(cancellationToken);

		return Result.Success(new DeleteOrderResult(true));
	}
}
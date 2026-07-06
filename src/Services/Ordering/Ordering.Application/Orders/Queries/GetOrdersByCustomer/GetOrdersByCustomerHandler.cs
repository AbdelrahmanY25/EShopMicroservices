namespace Ordering.Application.Orders.Queries.GetOrdersByCustomer;

public class GetOrdersByCustomerHandler(IAppDbContext dbContext) : IQueryHandler<GetOrdersByCustomerQuery, GetOrdersByCustomerResult>
{
	public async Task<Result<GetOrdersByCustomerResult>> Handle(GetOrdersByCustomerQuery query, CancellationToken cancellationToken)
	{
		var orders = await dbContext.Orders
			.Include(o => o.OrderItems)
			.AsNoTracking()
			.Where(o => o.CustomerId == CustomerId.Of(query.CustomerId))
			.OrderBy(o => o.OrderName.Value)
			.ToListAsync(cancellationToken);

		return Result.Success(new GetOrdersByCustomerResult(orders.ToOrderDtoList()));
	}
}
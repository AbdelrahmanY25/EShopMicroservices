namespace Ordering.Application.Orders.Queries.GetOrders;

public class GetOrdersHandler(IAppDbContext dbContext) : IQueryHandler<GetOrdersQuery, GetOrdersResult>
{
	public async Task<Result<GetOrdersResult>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
	{
		var orders = dbContext.Orders
			.Include(o => o.OrderItems)
			.AsNoTracking()
			.OrderBy(o => o.OrderName.Value);

		var paginatedOrders = await PaginatedResult<Order>
			.CreateAsync(orders, request.Pagination.PageSize, request.Pagination.PageNumber, cancellationToken);

		var response = new PaginatedResult<OrderDto>([.. paginatedOrders.Items.ToOrderDtoList()], paginatedOrders.TotalPages, paginatedOrders.PageNumber, request.Pagination.PageSize);

		return Result.Success(new GetOrdersResult(response));
	}
}
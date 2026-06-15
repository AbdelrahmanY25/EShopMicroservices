namespace Basket.Api.Basket.GetBasket;

public record GetBasketQuery(string UserName) : IQuery<GetBasketResult>;

public record GetBasketResult(ShoppingCart Cart);

public class GetBasketQueryHandler : IQueryHandler<GetBasketQuery, GetBasketResult>
{
	public async Task<Result<GetBasketResult>> Handle(GetBasketQuery query, CancellationToken cancellationToken)
	{
		// TODO: get basket from database

		return Result.Success(new GetBasketResult(new ShoppingCart(query.UserName)));
	}
}
namespace Basket.Api.Basket.GetBasket;

public record GetBasketQuery(string UserName) : IQuery<GetBasketResult>;

public record GetBasketResult(ShoppingCart Cart);

public class GetBasketQueryHandler(IBasketRepository basketRepository) : IQueryHandler<GetBasketQuery, GetBasketResult>
{
	private readonly IBasketRepository _basketRepository = basketRepository;

	public async Task<Result<GetBasketResult>> Handle(GetBasketQuery query, CancellationToken cancellationToken)
	{
		var basket = await _basketRepository.GetBasketAsync(query.UserName, cancellationToken);

		if (basket is null)
			return Result.Failure<GetBasketResult>(BasketErrors.NotFound);

		return Result.Success(new GetBasketResult(basket));
	}
}
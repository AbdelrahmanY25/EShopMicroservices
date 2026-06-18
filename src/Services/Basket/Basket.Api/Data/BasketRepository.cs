namespace Basket.Api.Data;

public class BasketRepository(IDocumentSession documentSession) : IBasketRepository
{
	private readonly IDocumentSession _session = documentSession;

	public async Task<ShoppingCart?> GetBasketAsync(string userName, CancellationToken cancellationToken = default)
	{
		var response = await _session.LoadAsync<ShoppingCart>(userName, cancellationToken);

		return response;
	}

	public async Task<ShoppingCart> StoreBasketAsync(ShoppingCart basket, CancellationToken cancellationToken = default)
	{
		_session.Store(basket);
		await _session.SaveChangesAsync(cancellationToken);
		return basket;
	}

	public async Task<bool> DeleteBasketAsync(string userName, CancellationToken cancellationToken = default)
	{
		var basket = await _session.LoadAsync<ShoppingCart>(userName, cancellationToken);

		if (basket is null)
			return false;

		_session.Delete(basket);
		await _session.SaveChangesAsync(cancellationToken);
		return true;
	}
}
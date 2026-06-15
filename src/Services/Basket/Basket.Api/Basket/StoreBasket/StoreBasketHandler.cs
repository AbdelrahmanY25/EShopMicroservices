namespace Basket.Api.Basket.StoreBasket;

public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;

public record StoreBasketResult(string UserName);

public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
{
	public StoreBasketCommandValidator()
	{
		RuleFor(x => x.Cart).NotNull().WithMessage("Cart cannot be null.");
		RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("UserName cannot be empty.");
	}
}

public class StoreBasketCommandHandler(IBasketRepository basketRepository) : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
	private readonly IBasketRepository _basketRepository = basketRepository;

	public async Task<Result<StoreBasketResult>> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
	{
		await _basketRepository.StoreBasketAsync(command.Cart, cancellationToken);

		return Result.Success(new StoreBasketResult(command.Cart.UserName));
	}
}
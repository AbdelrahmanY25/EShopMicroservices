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

public class StoreBasketCommandHandler : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
	public async Task<Result<StoreBasketResult>> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
	{
		// TODO: store basket in database

		return Result.Success(new StoreBasketResult(command.Cart.UserName));
	}
}
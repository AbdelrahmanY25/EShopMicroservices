using BuildingBlocks.Messaging.Events;
using MassTransit;

namespace Basket.Api.Basket.CheckoutBasket;

public record CheckoutBasketCommand(CheckoutBasketDto CheckoutBasketDto) : ICommand<CheckoutBasketResult>;

public record CheckoutBasketResult(bool IsSuccess);

public class CheckoutBasketCommandValidator : AbstractValidator<CheckoutBasketCommand>
{
	public CheckoutBasketCommandValidator()
	{
		RuleFor(x => x.CheckoutBasketDto).NotNull().WithMessage("BasketCheckoutDto can't be null");
		RuleFor(x => x.CheckoutBasketDto.UserName).NotEmpty().WithMessage("UserName is required");
	}
}

public class CheckoutBasketCommandHandler(IBasketRepository repository, IPublishEndpoint publishEndpoint) : ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult>
{
	public async Task<Result<CheckoutBasketResult>> Handle(CheckoutBasketCommand command, CancellationToken cancellationToken)
	{
		var basket = await repository.GetBasketAsync(command.CheckoutBasketDto.UserName, cancellationToken);

		if (basket is null)
			return Result.Failure<CheckoutBasketResult>(BasketErrors.NotFound);

		var eventMessage = command.CheckoutBasketDto.Adapt<BasketCheckoutEvent>();
		eventMessage.TotalPrice = basket.TotalPrice;

		await publishEndpoint.Publish(eventMessage, cancellationToken);

		await repository.DeleteBasketAsync(command.CheckoutBasketDto.UserName, cancellationToken);

		return Result.Success(new CheckoutBasketResult(true));
	}
}
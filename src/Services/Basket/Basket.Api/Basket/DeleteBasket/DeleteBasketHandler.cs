namespace Basket.Api.Basket.DeleteBasket;

public record DeleteBasketCommand(string UserName) : ICommand<DeleteBasketResult>;

public record DeleteBasketResult(bool IsSuccess);

public class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
{
	public DeleteBasketCommandValidator()
	{
		RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName cannot be empty.");
	}
}

public class DeleteBasketCommandHandler : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
{
	public async Task<Result<DeleteBasketResult>> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
	{
		// TODO: Delete the basket for the given UserName from the database or cache.

		return Result.Success(new DeleteBasketResult(true));
	}
}

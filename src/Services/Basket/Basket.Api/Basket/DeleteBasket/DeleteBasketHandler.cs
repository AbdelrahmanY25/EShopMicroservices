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

public class DeleteBasketCommandHandler(IBasketRepository basketRepository) : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
{
	private readonly IBasketRepository _basketRepository = basketRepository;

	public async Task<Result<DeleteBasketResult>> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
	{
		var isDeleted = await _basketRepository.DeleteBasketAsync(request.UserName, cancellationToken);

		if (!isDeleted)
			return Result.Failure<DeleteBasketResult>(BasketErrors.NotFound);

		return Result.Success(new DeleteBasketResult(true));
	}
}
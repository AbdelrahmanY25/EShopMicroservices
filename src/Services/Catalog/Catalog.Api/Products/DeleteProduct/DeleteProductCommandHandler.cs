namespace Catalog.Api.Products.DeleteProduct;

public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;

public record DeleteProductResult(bool IsSuccess);

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
	public DeleteProductCommandValidator()
	{
		RuleFor(x => x.Id).NotEmpty().WithMessage("Product ID is required");
	}
}

public class DeleteProductCommandHandler(IDocumentSession session) : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
	public async Task<Result<DeleteProductResult>> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
	{
		var product = await session.LoadAsync<Product>(command.Id, cancellationToken);

		if (product is null)
			return Result.Failure<DeleteProductResult>(ProductErrors.ProductNotFound);

		session.Delete(product);

		await session.SaveChangesAsync(cancellationToken);

		return Result.Success(new DeleteProductResult(true));
	}
}
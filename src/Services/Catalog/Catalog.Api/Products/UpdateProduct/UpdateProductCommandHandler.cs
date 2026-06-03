namespace Catalog.Api.Products.UpdateProduct;

public record UpdateProductCommand(Guid Id, string Name, string Description, decimal Price, List<string> Category, string ImageFile) : ICommand<UpdateProductResult>;

public record UpdateProductResult(bool IsSuccess);

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
	public UpdateProductCommandValidator()
	{
		RuleFor(command => command.Id).NotEmpty().WithMessage("Product ID is required");

		RuleFor(command => command.Name)
			.NotEmpty().WithMessage("Name is required")
			.Length(2, 150).WithMessage("Name must be between 2 and 150 characters");

		RuleFor(command => command.Price)
			.GreaterThan(0).WithMessage("Price must be greater than 0");
	}
}

public class UpdateProductCommandHandler(IDocumentSession session) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
	public async Task<Result<UpdateProductResult>> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
	{
		var product = await session.LoadAsync<Product>(command.Id, cancellationToken);

		if (product is null)
			return Result.Failure<UpdateProductResult>(ProductErrors.ProductNotFound);
		
		command.Adapt(product);

		session.Update(product);

		await session.SaveChangesAsync(cancellationToken);

		return Result.Success(new UpdateProductResult(true));
	}
}

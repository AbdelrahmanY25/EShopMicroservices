namespace Catalog.Api.Products.UpdateProduct;

public record UpdateProductCommand(Guid Id, string Name, string Description, decimal Price, List<string> Category, string ImageFile) : ICommand<UpdateProductResult>;

public record UpdateProductResult(bool IsSuccess);

public class UpdateProductCommandHandler(IDocumentSession session) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
	public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
	{
		var product = await session.LoadAsync<Product>(command.Id, cancellationToken) ??
			throw new ProductNotFoundException($"Product with id {command.Id} not found.");
		
		command.Adapt(product);

		session.Update(product);

		await session.SaveChangesAsync(cancellationToken);

		return new UpdateProductResult(true);
	}
}

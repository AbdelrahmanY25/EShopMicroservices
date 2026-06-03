namespace Catalog.Api.Errors;

public static class ProductErrors
{
	public static readonly Error ProductNotFound =
		new("ProductNotFound", "The specified product was not found.", StatusCodes.Status404NotFound);
}
namespace Basket.Api.Errors;

public static class BasketErrors
{
	public static readonly Error NotFound =
		new("BasketNotFound", "The specified basket was not found.", StatusCodes.Status404NotFound);
}
namespace Ordering.Application.Errors;

public static class OrderErrors
{
	public static readonly Error OrderNotFound =
		new("OrderNotFound", "The specified order was not found.", StatusCodes.Status404NotFound);
}
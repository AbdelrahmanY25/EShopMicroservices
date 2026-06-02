namespace Catalog.Api.Exceptions;

public class ProductNotFoundException(string message) : Exception(message)
{
}

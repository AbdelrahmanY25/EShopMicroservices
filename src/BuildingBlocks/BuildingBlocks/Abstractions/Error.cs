namespace BuildingBlocks.Abstractions;

public record Error(string Code, string Description, int? StatusCode)
{
	public static readonly Error None = new(string.Empty, string.Empty, null);

	public static Error Validation(string description) =>
		new("ValidationError", description, 400);
}
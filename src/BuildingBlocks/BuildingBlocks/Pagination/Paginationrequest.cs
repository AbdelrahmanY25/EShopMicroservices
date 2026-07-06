namespace BuildingBlocks.Pagination;

public record PaginationRequest(int PageNumber = 1, int PageSize = 10);

public class PaginationRequestValidator : AbstractValidator<PaginationRequest>
{
	public PaginationRequestValidator()
	{
		RuleFor(x => x.PageNumber)
			.GreaterThan(0).WithMessage("Page number must be greater than 0.");

		RuleFor(x => x.PageSize)
			.GreaterThan(0).WithMessage("Page size must be greater than 0.")
			.LessThanOrEqualTo(100).WithMessage("Page size must not exceed 100.");
	}
}	
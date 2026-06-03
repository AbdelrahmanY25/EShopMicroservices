using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.Extensions;

public static class ResultExtensions
{
	extension(Result result)
	{
		public IResult ToProblem()
		{
			if (result.IsSuccess)
				throw new InvalidOperationException("Can't convert a successful result to a problem.");

			return Results.Problem(
				statusCode: result.Error.StatusCode,
				title: result.Error.Code,
				detail: result.Error.Description,
				extensions: new Dictionary<string, object?>
				{
					{ "errors", new[] { result.Error } }
				});
		}
	}
}
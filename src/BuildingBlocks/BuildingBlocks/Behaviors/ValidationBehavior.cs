namespace BuildingBlocks.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
	where TRequest : ICommand<TResponse>
{
	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		var context = new ValidationContext<TRequest>(request);

		var validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));

		var failures = validationResults
			.Where(r => r.Errors.Count != 0)
			.SelectMany(f => f.Errors)
			.ToList();

		if (failures.Count != 0)
		{
			var errorDescription = string.Join("; ", failures.Select(f => f.ErrorMessage));
			var error = Error.Validation(errorDescription);

			// TResponse is Result (non-generic)
			if (typeof(TResponse) == typeof(Result))
				return (TResponse)(object)Result.Failure(error);

			// TResponse is Result<T> — get T and call Result.Failure<T>(error)
			var resultType = typeof(TResponse);
			if (resultType.IsGenericType && resultType.GetGenericTypeDefinition() == typeof(Result<>))
			{
				var failureMethod = typeof(Result)
					.GetMethod(nameof(Result.Failure), 1, [typeof(Error)])!
					.MakeGenericMethod(resultType.GetGenericArguments()[0]);

				return (TResponse)failureMethod.Invoke(null, [error])!;
			}

			// Fallback for non-Result response types
			throw new ValidationException(failures);
		}

		return await next(cancellationToken);
	}
}
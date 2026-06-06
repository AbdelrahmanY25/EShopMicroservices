namespace BuildingBlocks.Behaviors;

public class ValidationBehavior<TRequest, TResult>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResult>
	where TRequest : notnull, IRequest<TResult>
{
	public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken)
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

			// TResult is Result (non-generic)
			if (typeof(TResult) == typeof(Result))
				return (TResult)(object)Result.Failure(error);

			// TResult is Result<T> — get T and call Result.Failure<T>(error)
			var resultType = typeof(TResult);

			if (resultType.IsGenericType && resultType.GetGenericTypeDefinition() == typeof(Result<>))
			{
				var failureMethod = typeof(Result)
					.GetMethod(nameof(Result.Failure), 1, [typeof(Error)])!
					.MakeGenericMethod(resultType.GetGenericArguments());

				return (TResult)failureMethod.Invoke(null, [error])!;
			}

			// Fallback for non-Result response types
			throw new ValidationException(failures);
		}

		return await next(cancellationToken);
	}
}
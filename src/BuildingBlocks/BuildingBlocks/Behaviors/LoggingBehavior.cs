namespace BuildingBlocks.Behaviors;

public class LoggingBehavior<TRequest, TResult>(ILogger<LoggingBehavior<TRequest, TResult>> logger) : IPipelineBehavior<TRequest, TResult>
	where TRequest : notnull, IRequest<TResult>
{
	public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken)
	{
		logger.LogInformation("[START] Handle request={Request} - Response={Response} - RequestData={RequestData}",
			typeof(TRequest).Name, typeof(TResult).Name, request);

		var timer = new Stopwatch();
		timer.Start();

		var response = await next(cancellationToken);

		timer.Stop();
		var timeTaken = timer.Elapsed;

		if (timeTaken.Seconds > 3) // if the request is greater than 3 seconds, then log the warnings
			logger.LogWarning("[PERFORMANCE] The request {Request} took {TimeTaken} seconds.",
				typeof(TRequest).Name, timeTaken.Seconds);

		logger.LogInformation("[END] Handled {Request} with {Response}", typeof(TRequest).Name, typeof(TResult).Name);
		return response;
	}
}
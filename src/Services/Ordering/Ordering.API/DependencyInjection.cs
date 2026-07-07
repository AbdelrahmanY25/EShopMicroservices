namespace Ordering.API;

public static class DependencyInjection
{
	extension(IServiceCollection services)
	{
		public IServiceCollection AddApiServices()
		{
			services.AddCarter();

			services.AddExceptionHandler<GlobalExceptionHandler>();
			
			services.AddProblemDetails();

			return services;
		}
	}
}
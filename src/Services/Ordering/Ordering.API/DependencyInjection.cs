namespace Ordering.API;

public static class DependencyInjection
{
	extension(IServiceCollection services)
	{
		public IServiceCollection AddApiServices(IConfiguration configuration)
		{
			var connectionString = configuration.GetConnectionString("Database")!;

			services.AddCarter();

			services.AddExceptionHandler<GlobalExceptionHandler>();
			
			services.AddProblemDetails();

			services.AddHealthChecks()
				.AddSqlServer(connectionString, name: "Sql Server");

			return services;
		}
	}
}
namespace Ordering.Infrastructure;

public static class DependencyInjection
{
	extension(IServiceCollection services)
	{
		public IServiceCollection AddInfrastructureServices(IConfiguration configuration)
		{
			var connectionString = configuration.GetConnectionString("Database");

			services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

			return services;
		}
	}
}
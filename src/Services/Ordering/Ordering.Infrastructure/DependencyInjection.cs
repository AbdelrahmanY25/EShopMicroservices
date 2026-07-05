namespace Ordering.Infrastructure;

public static class DependencyInjection
{
	extension(IServiceCollection services)
	{
		public IServiceCollection AddInfrastructureServices(IConfiguration configuration)
		{
			var connectionString = configuration.GetConnectionString("Database");

			services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

			services.AddDbContext<AppDbContext>((sp, options) => 
			{
				options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
				options.UseSqlServer(connectionString);
			});

			return services;
		}
	}
}
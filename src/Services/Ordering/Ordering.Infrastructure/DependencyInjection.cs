namespace Ordering.Infrastructure;

public static class DependencyInjection
{
	extension(IServiceCollection services)
	{
		public IServiceCollection AddInfrastructureServices(IConfiguration configuration)
		{
			return services;
		}
	}
}
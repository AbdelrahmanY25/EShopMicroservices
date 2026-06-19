namespace Ordering.Application;

public static class DependencyInjection
{
	extension(IServiceCollection services)
	{
		public IServiceCollection AddApplicationServices()
		{
			return services;
		}
	}
}
namespace Ordering.API;

public static class DependencyInjection
{
	extension(IServiceCollection services)
	{
		public IServiceCollection AddApiServices()
		{
			services.AddCarter();

			return services;
		}
	}
}
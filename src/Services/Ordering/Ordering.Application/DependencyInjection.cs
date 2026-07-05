namespace Ordering.Application;

public static class DependencyInjection
{
	extension(IServiceCollection services)
	{
		public IServiceCollection AddApplicationServices()
		{
			services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

			return services;
		}
	}
}
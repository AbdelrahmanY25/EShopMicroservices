namespace Ordering.Application;

public static class DependencyInjection
{
	extension(IServiceCollection services)
	{
		public IServiceCollection AddApplicationServices()
		{
			services.AddMediatR(config => 
			{
				config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
				config.AddOpenBehavior(typeof(LoggingBehavior<,>));
				config.AddOpenBehavior(typeof(ValidationBehavior<,>));
			});

			return services;
		}
	}
}
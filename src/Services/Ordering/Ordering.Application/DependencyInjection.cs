namespace Ordering.Application;

public static class DependencyInjection
{
	extension(IServiceCollection services)
	{
		public IServiceCollection AddApplicationServices(IConfiguration configuration)
		{
			services.AddMediatR(config => 
			{
				config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
				config.AddOpenBehavior(typeof(LoggingBehavior<,>));
				config.AddOpenBehavior(typeof(ValidationBehavior<,>));
			});

			services.AddMessageBroker(configuration, Assembly.GetExecutingAssembly());

			return services;
		}
	}
}
namespace Discount.Grpc.Presistance;

public static class Extensions
{
	extension(IApplicationBuilder app) 
	{
		public IApplicationBuilder UseMigration()
		{
			using var scope = app.ApplicationServices.CreateScope();
			using var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
			dbContext.Database.Migrate();

			return app;
		}
	}
}
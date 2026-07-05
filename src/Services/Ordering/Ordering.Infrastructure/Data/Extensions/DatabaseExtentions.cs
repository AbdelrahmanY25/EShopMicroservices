namespace Ordering.Infrastructure.Data.Extensions;

public static class DatabaseExtentions
{
	extension(WebApplication app)
	{
		public async Task InitialiseDatabaseAsync()
		{
			using var scope = app.Services.CreateScope();
			var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
			await dbContext.Database.MigrateAsync();

			await SeedAsync(dbContext);
		}
	}

	private static async Task SeedAsync(AppDbContext context)
	{
		await SeedCustomerAsync(context);
		await SeedProductAsync(context);
		await SeedOrdersWithItemsAsync(context);
	}

	private static async Task SeedCustomerAsync(AppDbContext context)
	{
		if (!await context.Customers.AnyAsync())
		{
			await context.Customers.AddRangeAsync(InitialData.Customers);
			await context.SaveChangesAsync();
		}
	}

	private static async Task SeedProductAsync(AppDbContext context)
	{
		if (!await context.Products.AnyAsync())
		{
			await context.Products.AddRangeAsync(InitialData.Products);
			await context.SaveChangesAsync();
		}
	}

	private static async Task SeedOrdersWithItemsAsync(AppDbContext context)
	{
		if (!await context.Orders.AnyAsync())
		{
			await context.Orders.AddRangeAsync(InitialData.OrdersWithItems);
			await context.SaveChangesAsync();
		}
	}
}
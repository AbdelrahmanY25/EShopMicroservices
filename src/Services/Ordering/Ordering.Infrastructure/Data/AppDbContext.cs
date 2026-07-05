namespace Ordering.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IAppDbContext
{
	public DbSet<Customer> Customers => Set<Customer>();
	public DbSet<Product> Products => Set<Product>();
	public DbSet<Order> Orders => Set<Order>();
	public DbSet<OrderItem> OrderItems => Set<OrderItem>();

	protected override void OnModelCreating(ModelBuilder builder)
	{
		builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
		base.OnModelCreating(builder);
	}

	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		var entries = ChangeTracker.Entries<IEntity>();

		foreach (var entry in entries)
		{
			if (entry.State == EntityState.Added)
			{
				entry.Property(e => e.CreatedBy).CurrentValue = "System";
				entry.Property(e => e.CreatedAt).CurrentValue = DateTime.UtcNow;
			}
			else if (entry.State == EntityState.Modified)
			{
				entry.Property(e => e.LastModifiedBy).CurrentValue = "System";
				entry.Property(e => e.LastModified).CurrentValue = DateTime.UtcNow;
			}
		}

		return base.SaveChangesAsync(cancellationToken);
	}
}
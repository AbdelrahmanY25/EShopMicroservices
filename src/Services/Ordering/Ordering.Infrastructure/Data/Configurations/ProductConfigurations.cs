namespace Ordering.Infrastructure.Data.Configurations;

public class ProductConfigurations : IEntityTypeConfiguration<Product>
{
	public void Configure(EntityTypeBuilder<Product> builder)
	{
		builder.HasKey(p => p.Id);

		builder.ToTable("Products", "oms");

		builder.Property(p => p.Id)
		       .HasConversion(
					productId => productId.Value,
					dbId => ProductId.Of(dbId)
			   );

		builder.Property(p => p.Name).HasMaxLength(100).IsRequired();

		builder.Property(p => p.Price).HasColumnType("decimal(18,2)").IsRequired();
	}
}
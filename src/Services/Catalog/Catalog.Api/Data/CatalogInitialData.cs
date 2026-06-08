using Marten.Schema;

namespace Catalog.Api.Data;

public class CatalogInitialData : IInitialData
{
	public async Task Populate(IDocumentStore store, CancellationToken cancellation)
	{
		using var session = store.LightweightSession();

		if (await session.Query<Product>().AnyAsync(cancellation))
			return;

		session.Store(GetPreconfiguredProducts());
		await session.SaveChangesAsync(cancellation);
	}

	private static IEnumerable<Product> GetPreconfiguredProducts() =>
			[
				new Product()
				{
					Id = Guid.CreateVersion7(),
					Name = "IPhone X",
					Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
					ImageFile = "product-1.png",
					Price = 950.00M,
					Category = ["Smart Phone"]
				},
				new Product()
				{
					Id = Guid.CreateVersion7(),
					Name = "Samsung 10",
					Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
					ImageFile = "product-2.png",
					Price = 840.00M,
					Category = ["Smart Phone"]
				},
				new Product()
				{
					Id = Guid.CreateVersion7(),
					Name = "Huawei Plus",
					Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
					ImageFile = "product-3.png",
					Price = 650.00M,
					Category = ["White Appliances"]
				},
				new Product()
				{
					Id = Guid.CreateVersion7(),
					Name = "Xiaomi Mi 9",
					Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
					ImageFile = "product-4.png",
					Price = 470.00M,
					Category = ["White Appliances"]
				},
				new Product()
				{
					Id = Guid.CreateVersion7(),
					Name = "HTC U11+ Plus",
					Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
					ImageFile = "product-5.png",
					Price = 380.00M,
					Category = ["Smart Phone"]
				},
				new Product()
				{
					Id = Guid.CreateVersion7(),
					Name = "LG G7 ThinQ",
					Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
					ImageFile = "product-6.png",
					Price = 240.00M,
					Category = ["Home Kitchen"]
				},
				new Product()
				{
					Id = Guid.CreateVersion7(),
					Name = "Panasonic Lumix",
					Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
					ImageFile = "product-6.png",
					Price = 240.00M,
					Category = ["Camera"]
				}
			];
}
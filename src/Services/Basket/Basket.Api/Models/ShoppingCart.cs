namespace Basket.Api.Models;

public class ShoppingCart
{
	public string UserName { get; set; } = string.Empty;
	public List<ShoppingCartItem> Items { get; set; } = [];
	public decimal TotalPrice => Items.Sum(i => i.Price * i.Quantity);

	public ShoppingCart(string userName)
	{
		UserName = userName;
	}

	// For Mapping
	public ShoppingCart()
	{
		
	}
}
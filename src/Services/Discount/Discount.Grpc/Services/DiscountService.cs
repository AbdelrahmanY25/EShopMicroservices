namespace Discount.Grpc.Services;

public class DiscountService(AppDbContext dbContext, ILogger<DiscountService> logger) : DiscountProtoService.DiscountProtoServiceBase
{
	private readonly AppDbContext _dbContext = dbContext;
	private readonly ILogger<DiscountService> _logger = logger;

	public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
	{
		var coupon = await _dbContext.Coupons
			.FirstOrDefaultAsync(c => c.ProductName == request.ProductName);

		coupon ??= new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount Desc" };

		_logger.LogInformation("Discount for ProductName : {productName}, Amount : {amount}", coupon.ProductName, coupon.Amount);

		var couponModel = coupon.Adapt<CouponModel>();

		return couponModel;
	}

	public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
	{
		var coupon = request.Coupon.Adapt<Coupon>() ??
			throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object."));

		_dbContext.Coupons.Add(coupon);
		await _dbContext.SaveChangesAsync();

		_logger.LogInformation("Discount is successfully created. ProductName : {ProductName}", coupon.ProductName);

		var couponModel = coupon.Adapt<CouponModel>();

		return couponModel;
	}

	public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
	{
		var oldCoupon = await _dbContext.Coupons.FindAsync(request.Coupon.Id) ??
			throw new RpcException(new Status(StatusCode.NotFound, $"Discount with Id={request.Coupon.Id} is not found."));

		request.Coupon.Adapt(oldCoupon);

		await _dbContext.SaveChangesAsync();

		_logger.LogInformation("Discount is successfully updated. ProductName : {ProductName}", oldCoupon.ProductName);

		return oldCoupon.Adapt<CouponModel>();
	}

	public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
	{
		var coupon = await _dbContext.Coupons
			.FirstOrDefaultAsync(c => c.ProductName == request.ProductName) ??
			throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.ProductName} is not found."));

		_dbContext.Coupons.Remove(coupon);
		await _dbContext.SaveChangesAsync();

		_logger.LogInformation("Discount is successfully deleted. ProductName : {ProductName}", coupon.ProductName);

		return new DeleteDiscountResponse { Success = true };
	}
}
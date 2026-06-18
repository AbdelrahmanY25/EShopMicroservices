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

		_logger.LogInformation("Discount is retrieved for ProductName : {productName}, Amount : {amount}", coupon.ProductName, coupon.Amount);

		var couponModel = coupon.Adapt<CouponModel>();

		return couponModel;
	}

	public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
	{
		var coupon = request.Adapt<Coupon>() ??
			throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object."));

		_dbContext.Coupons.Add(coupon);
		await _dbContext.SaveChangesAsync();

		_logger.LogInformation("Discount is successfully created. ProductName : {ProductName}", coupon.ProductName);

		var couponModel = coupon.Adapt<CouponModel>();

		return couponModel;
	}

	public override Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
	{
		return base.UpdateDiscount(request, context);
	}

	public override Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
	{
		return base.DeleteDiscount(request, context);
	}
}
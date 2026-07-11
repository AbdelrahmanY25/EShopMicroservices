using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddReverseProxy()
	.LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));


builder.Services.AddRateLimiter(rateLimiterOptions =>
{
	rateLimiterOptions.AddFixedWindowLimiter("fixed", options =>
	{
		options.Window = TimeSpan.FromSeconds(10);
		options.PermitLimit = 5;
	});
});

builder.Services.AddOutputCache(options =>
{
	options.AddPolicy("customPolicy", builder => builder.Expire(TimeSpan.FromSeconds(20)));
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseRateLimiter();

app.UseOutputCache();

app.MapReverseProxy();

app.Run();
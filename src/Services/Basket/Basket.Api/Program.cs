var builder = WebApplication.CreateBuilder(args);

var assemply = Assembly.GetExecutingAssembly();

var connectionString = builder.Configuration.GetConnectionString("Database")!;

var redisConnectionString = builder.Configuration.GetConnectionString("Redis")!;

// Add services to the container.

builder.Services.AddCarter();

builder.Services.AddMediatR(cfg =>
{
	cfg.RegisterServicesFromAssembly(assemply);
	cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
	cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(assemply);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddProblemDetails();

builder.Services.AddMarten(options =>
{
	options.Connection(connectionString);
	options.Schema.For<ShoppingCart>().Identity(x => x.UserName);
})
.UseLightweightSessions();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();

builder.Services.AddStackExchangeRedisCache(options =>
{
	options.Configuration = redisConnectionString;
});

builder.Services.AddHealthChecks()
	.AddNpgSql(connectionString, name: "PostgreSQL")
	.AddRedis(redisConnectionString, name: "Redis");

builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
{
	options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);
})
.ConfigurePrimaryHttpMessageHandler(() =>
{
	var handler = new HttpClientHandler
	{
		ServerCertificateCustomValidationCallback =
		HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
	};

	return handler;
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapCarter();

app.UseExceptionHandler();

app.MapHealthChecks("/health", new HealthCheckOptions
{
	ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
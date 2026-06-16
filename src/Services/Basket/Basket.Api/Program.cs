var builder = WebApplication.CreateBuilder(args);

var assemply = Assembly.GetExecutingAssembly();

var connectionString = builder.Configuration.GetConnectionString("Database")!;

var RedisConnectionString = builder.Configuration.GetConnectionString("Redis")!;

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
	options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

builder.Services.AddHealthChecks()
	.AddNpgSql(connectionString, name: "PostgreSQL")
	.AddRedis(RedisConnectionString, name: "Redis");

// Configure the HTTP request pipeline.

var app = builder.Build();

app.MapCarter();

app.UseExceptionHandler();

app.MapHealthChecks("/health", new HealthCheckOptions
{
	ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
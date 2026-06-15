var builder = WebApplication.CreateBuilder(args);

var assemply = Assembly.GetExecutingAssembly();

var connectionString = builder.Configuration.GetConnectionString("Database")!;

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

// Configure the HTTP request pipeline.

var app = builder.Build();

app.MapCarter();

app.UseExceptionHandler();

app.Run();
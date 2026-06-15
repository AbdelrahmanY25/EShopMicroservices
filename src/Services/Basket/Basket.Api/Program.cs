using BuildingBlocks.Exceptions;

var builder = WebApplication.CreateBuilder(args);

var assemply = Assembly.GetExecutingAssembly();

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

// Configure the HTTP request pipeline.

var app = builder.Build();

app.MapCarter();

app.UseExceptionHandler();

app.Run();
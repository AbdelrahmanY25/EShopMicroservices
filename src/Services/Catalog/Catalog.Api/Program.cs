var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var assemply = typeof(Program).Assembly;

builder.Services.AddMediatR(cfg => 
{
	cfg.RegisterServicesFromAssembly(assemply);
	cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(assemply);

builder.Services.AddCarter();

builder.Services.AddMarten(options => {
	options.Connection(builder.Configuration.GetConnectionString("Database")!);
})
.UseLightweightSessions();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapCarter();
app.Run();
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var assemply = Assembly.GetExecutingAssembly();
var connectionString = builder.Configuration.GetConnectionString("Database")!;

builder.Services.AddMediatR(cfg => 
{
	cfg.RegisterServicesFromAssembly(assemply);
	cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
	cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(assemply);

builder.Host.UseSerilog((context, configuration) =>
	configuration.ReadFrom.Configuration(context.Configuration)
);

builder.Services.AddCarter();

builder.Services.AddMarten(options =>
{
	options.Connection(connectionString);
})
.UseLightweightSessions();

if (builder.Environment.IsDevelopment())
	builder.Services.InitializeMartenWith<CatalogInitialData>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddProblemDetails();

builder.Services.AddHealthChecks()
	.AddNpgSql(connectionString);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapCarter();

app.UseSerilogRequestLogging();

app.UseExceptionHandler();

app.MapHealthChecks("/health", new HealthCheckOptions 
	{ 
		ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
	});

app.Run();
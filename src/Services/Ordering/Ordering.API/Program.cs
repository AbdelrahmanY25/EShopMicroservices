var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
	.AddApiServices(builder.Configuration)
	.AddApplicationServices()
	.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
	await app.InitialiseDatabaseAsync();
}

app.MapCarter();

app.UseExceptionHandler();

app.MapHealthChecks("/health", new HealthCheckOptions
{
	ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
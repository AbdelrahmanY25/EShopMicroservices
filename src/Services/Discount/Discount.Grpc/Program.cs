var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Database");

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));

builder.Services.AddGrpc();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseMigration();

app.MapGrpcService<DiscountService>();

app.UseExceptionHandler();

app.Run();
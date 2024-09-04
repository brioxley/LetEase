using LetEase.API;
using Microsoft.EntityFrameworkCore;
using LetEase.Infrastructure.Data;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Test database connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
using (var connection = new SqlConnection(connectionString))
{
	try
	{
		connection.Open();
		Console.WriteLine("Successfully connected to the database.");
	}
	catch (SqlException e)
	{
		Console.WriteLine($"Failed to connect to the database. Error: {e.Message}");
	}
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Add this block to test database connection
using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;
	try
	{
		var context = services.GetRequiredService<ApplicationDbContext>();
		context.Database.Migrate();
		app.Logger.LogInformation("Database connection successful and migrations applied.");
	}
	catch (Exception ex)
	{
		app.Logger.LogError(ex, "An error occurred while migrating or initializing the database.");
	}
}

app.Run();
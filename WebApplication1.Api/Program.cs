using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using WebApplication1.API.Data;
using WebApplication1.API.Routes;
using Scalar.AspNetCore;
using WebApplication1.API.Domain;
using WebApplication1.API.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = String.IsNullOrEmpty(builder.Configuration.GetConnectionString("DefaultConnection"))
        ? builder.Configuration["DB:ConnectionString"]
          ?? throw new InvalidOperationException("Connection string not found")
        : builder.Configuration.GetConnectionString("DefaultConnection")
          ?? throw new InvalidOperationException("Connection string not found");
    options.UseNpgsql(connectionString);
});
builder.Services.AddScoped<IRepository<Studio>, Repository<Studio>>();
builder.Services.AddScoped<IRepository<Game>, Repository<Game>>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.UseHttpsRedirection();
}

app.MapAppRoutes();

app.UseExceptionHandler(exceptionHandlerApp 
    => exceptionHandlerApp.Run(async context 
        => await Results.Problem(context.Features.Get<IExceptionHandlerPathFeature>()?.Error.Message)
                     .ExecuteAsync(context)));

app.Run();


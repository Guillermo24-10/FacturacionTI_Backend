using FacturacionTI.Application.Extensions;
using FacturacionTI.Infrastructure.Exntesions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// agregar injection de dependecias de capas
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();

var Cors = "Permitir";

builder.Services.AddCors(options =>
{
    options.AddPolicy(Cors, builder =>
    {
        builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseCors(Cors);

app.UseAuthorization();

app.MapControllers();

app.Run();

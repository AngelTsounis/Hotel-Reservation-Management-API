using Hotel.Reservation.Management.API.Endpoints;
using Hotel.Reservation.Management.API.Handlers;
using Hotel.Reservation.Management.Application;
using Hotel.Reservation.Management.Domain.Exceptions;
using Hotel.Reservation.Management.Infrastructure;
using Hotel.Reservation.Management.Infrastructure.Persistence;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.ConfigureHttpJsonOptions(options =>
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

//Docker Helper
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    try
    {
        logger.LogInformation("Applying database migrations.");
        await dbContext.Database.MigrateAsync();
        logger.LogInformation("Database migrations applied successfully.");
    }
    catch (Exception ex)
    {
        logger.LogCritical(ex, "Database migration failed. The application cannot start.");
        throw;
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "Hotel Reservation Management v1"));
}

app.UseExceptionHandler(handler => handler.Run(async context =>
{
    var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

    var statusCode = exception switch
    {
        BadHttpRequestException => StatusCodes.Status400BadRequest,
        NotFoundException => StatusCodes.Status404NotFound,
        ConflictException => StatusCodes.Status409Conflict,
        BusinessRuleException => StatusCodes.Status400BadRequest,
        _ => StatusCodes.Status500InternalServerError
    };

    context.Response.StatusCode = statusCode;

    await context.Response.WriteAsJsonAsync(new ProblemDetails
    {
        Status = statusCode,
        Title = statusCode == StatusCodes.Status500InternalServerError
            ? "An unexpected error occurred."
            : "Request could not be completed.",
        Detail = statusCode == StatusCodes.Status500InternalServerError
            ? null
            : exception?.Message
    });
}));

if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") != "true")
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

app.MapApiEndpoints();

app.Run();

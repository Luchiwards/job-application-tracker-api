using JobApplicationTracker.Api.Common.Middleware;
using JobApplicationTracker.Api.Extensions;
using JobApplicationTracker.Application;
using JobApplicationTracker.Infrastructure;
using JobApplicationTracker.Infrastructure.Logging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilogLogging();

builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddPresentationServices();

builder.Services.AddAuthorization();

var app = builder.Build();

await app.Services.InitialiseDatabaseAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseCors(PresentationServiceRegistration.GetCorsPolicyName());

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
/// <summary>
/// Provides access to the application's entry point for integration testing scenarios.
/// </summary>
public partial class Program;

using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using ServerWebApi;
using ServerWebApi.Repositories;
using System.Diagnostics;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<PostgresConfiguration>(configuration.GetSection("ConnectionStrings"));
builder.Services.AddTransient<IValueRepository, ValueRepository>();

var serviceName = Assembly.GetExecutingAssembly().GetName().FullName;
builder.Services.AddSingleton<ActivitySource>(new ActivitySource(serviceName));
builder.Services.AddOpenTelemetryTracing(traceBuilder =>
{
    traceBuilder
        .AddConsoleExporter()
        .AddSource(serviceName)
        .SetResourceBuilder(
            ResourceBuilder.CreateDefault()
                .AddService(serviceName))
        .AddAspNetCoreInstrumentation()
        .AddOtlpExporter(config => config.Endpoint = new Uri("http://otelcollector:4317"));//todo move to config

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

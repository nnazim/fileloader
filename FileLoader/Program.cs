using Amazon.Util;
using FileLoader.Middleware;
using FileLoader.Services;
using FileLoader.Services.Factory;
using FileLoader.Services.Processors;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace FileLoader
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Create a JSON-formatted logger
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.WithEnvironmentName()
                .Enrich.WithThreadId()
                .Enrich.WithProcessId()
                .MinimumLevel.Information()
                .WriteTo.Console(new RenderedCompactJsonFormatter()) // JSON format for CloudWatch
                .CreateLogger();


            builder.Host.UseSerilog();

            // Add services to the container.
            builder.Services.AddControllers();

            // Register all processors
            builder.Services.AddTransient<IFileProcessor, MassCardCreationProcessor>();
            builder.Services.AddTransient<IFileProcessor, TollTransactionProcessor>();
            // builder.Services.AddTransient<IFileProcessor, JustificationProcessor>();

            // Factory + Service
            builder.Services.AddSingleton<IFileProcessorFactory, FileProcessorFactory>();
            builder.Services.AddTransient<FileProcessingService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            // Add middleware to include correlation ID
            app.UseMiddleware<CorrelationIdMiddleware>();
            app.UseMiddleware<RequestResponseLoggingMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}

using hackernews.Core;
using hackernews.Core.Config;
using hackernews.Core.Model;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Formatting.Json;


namespace hackernews
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var configuration = builder.Configuration.AddJsonFile("appsettings.json", false).Build();
            builder.Services.AddHttpClient(MagicStrings.HackerNewsHttpClientFactory, c =>
            {
                c.BaseAddress = new Uri(configuration.GetSection(HackerNewsConfig.Section).Get<HackerNewsConfig>().BaseUri);
            });
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.RegisterServices();
            builder.Services.AddMemoryCache();
            builder.Services.AddControllers();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HackerNews Story Retriver Service", Version = "v1" });
            });

            Log.Logger = new LoggerConfiguration().WriteTo.Console(new JsonFormatter())
            .CreateLogger();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            
            app.MapControllers();
            app.UseHttpsRedirection();


            try
            {
                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}

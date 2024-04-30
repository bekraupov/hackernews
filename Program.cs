using Serilog;
using Serilog.Formatting.Json;


namespace hackernews
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            app.MapGet("/", () => "Hello World!");

            Log.Logger = new LoggerConfiguration().WriteTo.Console(new JsonFormatter())
            .CreateLogger();

            app.Run();
        }
    }
}

using DOwithStop.Data;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace DOwithStop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ////create the logger and setup your sinks, filters and properties
            Log.Logger = new LoggerConfiguration()
                            
                            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                            .Enrich.FromLogContext()
                            .WriteTo.Console()
                            // Add this line:
                            .WriteTo.File(
                               System.IO.Path.Combine(Environment.CurrentDirectory,"diagnostics.txt"), 
                               rollingInterval: RollingInterval.Day)
                             
                            .CreateLogger();


            // Add services to the container.
            // REGISTER SERVICES HERE

            try
            {
                Log.Information("Starting web application");

                builder.Services.AddControllersWithViews();
                builder.Services.AddDbContext<CustomDBContext>();

                //after create the builder - UseSerilog
                builder.Host.UseSerilog();
                var app = builder.Build();




                // REGISTER MIDDLEWARE HERE
                // Configure the HTTP request pipeline.
                if (!app.Environment.IsDevelopment())
                {
                    app.UseExceptionHandler("/Home/Error");
                    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                    app.UseHsts();
                }
               
                app.UseHttpsRedirection();
                app.UseStaticFiles();
                app.UseSerilogRequestLogging();
                app.UseRouting();

                app.UseAuthorization();

                app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Dashboard}/{action=Index}/{id?}");
                app.MapControllerRoute(
                    name: "DefaultApi",
                    pattern: "api/{controller}/{id}");

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }

        }
    }
}
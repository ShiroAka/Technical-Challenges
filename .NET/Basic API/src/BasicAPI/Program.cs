using System.Diagnostics.CodeAnalysis;
using BasicAPI.Interfaces;
using BasicAPI.Services;
using NLog;
using NLog.Web;

namespace BasicAPI
{
    [ExcludeFromCodeCoverage]
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = LogManager.Setup().LoadConfigurationFromFile("nlog.config").GetCurrentClassLogger();

            try
            {
                logger.Debug("Init Main");

                var builder = WebApplication.CreateBuilder(args);

                builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddEnvironmentVariables();

                builder.Services.AddSingleton<IDeviceService, DeviceService>();
                builder.Services.AddSingleton<IUserService, UserService>();
                builder.Services.AddControllers();
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
                builder.Host.UseNLog();

                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();
                app.UseAuthorization(); //TODO - Add a way to authorize based on a custom header
                app.MapControllers();

                app.Run();
            }
            catch (Exception ex)
            {
                //NLog: catch setup errors
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                LogManager.Shutdown();
            }                        
        }
    }
}
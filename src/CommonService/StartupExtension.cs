using System.Reflection;
using NetCore.AutoRegisterDi;
using Serilog;
using Serilog.Core;
using Serilog.Debugging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace CommonService{
    public static class StartupExtension{
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        private static string CommonOrigin = "CommonOrigin";
        public static IServiceCollection InjectServicesAndRepositories(this IServiceCollection services, Assembly assembly){
            var namespaceValue = Configuration.GetValue<string>("ProjectNamespace");
            services.RegisterAssemblyPublicNonGenericClasses(assembly)
            .Where(c => c.FullName.StartsWith(namespaceValue) && (c.Name.EndsWith("Service") || c.Name.EndsWith("Repository")))
                .AsPublicImplementedInterfaces();
            return services;
        }

        public static void UseCommonApplicationBuilder(this IApplicationBuilder app)
        {
            app.UseCors(CommonOrigin);
            app.UseApiVersioning();
            app.UseExceptionHandler("/error-local-development");
            var swaggerConfig = Configuration.GetSection("Swagger");
            if (swaggerConfig.GetValue<bool?>("Enabled").HasValue && swaggerConfig.GetValue<bool?>("Enabled").Value)
            {
                string svrUrl = swaggerConfig.GetValue<string>("OpenApiServerUrl");
                app.UseSwagger(c =>
                {
                    c.PreSerializeFilters.Add((swagger, httpReq) =>
                    {
                        if (String.IsNullOrEmpty(svrUrl))
                        {
                            if (!String.IsNullOrEmpty(httpReq.Headers["X-Forwarded-Proto"]))
                                svrUrl = $"{httpReq.Headers["X-Forwarded-Proto"]}://{httpReq.Headers["X-Forwarded-Host"]}/{httpReq.Headers["X-Forwarded-Prefix"]}";
                            else
                                svrUrl = $"{httpReq.Scheme}://{httpReq.Host.Value}/{httpReq.PathBase}";
                        }
                        swagger.Servers = new List<OpenApiServer>
                        {
                            new OpenApiServer { Url = svrUrl }
                        };
                    });
                });
                var sEndPoint = swaggerConfig.GetValue<string>("EndPoint");
                sEndPoint = $"{svrUrl}{(!String.IsNullOrEmpty(sEndPoint) ? sEndPoint : "/swagger/v1/swagger.json")}";
                app.UseSwaggerUI(c =>
                {
                    c.RoutePrefix = swaggerConfig.GetValue<string>("RoutePrefix");
                    c.SwaggerEndpoint(sEndPoint, swaggerConfig.GetValue<string>("Title"));
                });
            }


            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
        }

        public static IHostBuilder UseCommonHostBuilder(this IHostBuilder host)
        {
            var levelSwitch = new LoggingLevelSwitch();
            SelfLog.Enable(Console.Error);
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(levelSwitch)
                .WriteTo.File("logs/myapp.txt", rollingInterval: RollingInterval.Day)
                .Enrich.WithProperty("Application", Configuration.GetValue<string>("Project"))
                .Enrich.WithProperty("Environment", Configuration.GetValue<string>("Environment"))
                .Enrich.FromLogContext()
                .WriteTo.Debug()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
                .CreateLogger();
            host.UseSerilog();

            try
            {
                Log.Information($"Starting {Configuration.GetValue<string>("Project")}");

                host.Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, $"Startup {Configuration.GetValue<string>("Project")} failured!");
            }
            finally
            {
                Log.CloseAndFlush();
            }

            return host;
        }
    }
}

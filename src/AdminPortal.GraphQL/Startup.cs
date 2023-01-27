using AdminPortal.Data;
using AdminPortal.GraphQL.Authorization;
using CommonService;
using CommonService.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Reflection;
using System.Text;

namespace AdminPortal.GraphQL
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddGraphQLServer()
                .AddGraphQLTypes()
                .AddQueryType();
                //.AddMutationType()
                //.AddSubscriptionType();

            // Services
            //services.AddDbContext<Context>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContextFactory<Context>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
                    .LogTo(Log.Information)
                    .EnableSensitiveDataLogging(true)
                    .EnableDetailedErrors(true);
            });

            services.AddHttpClient();
            services.AddHttpContextAccessor();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            // Auth Services
            services.AddTransient<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddSingleton<IAuthorizationHandler, AdminAuthorizationHandler>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("SkAuthorization", policy => policy.Requirements.Add(new AdminAuthorizationRequirement()));
            });

            services.InjectServicesAndRepositories(Assembly.Load("AdminPortal.Business"));
            var corsConfig = Configuration.GetSection("Cors");
            services.AddCors(options =>
            {
                options.AddPolicy(
                    name: "CommonCors",
                    builder =>
                    {
                        if (corsConfig.GetValue<bool>("AllowAny"))
                        {
                            builder.AllowAnyOrigin()
                                .AllowAnyMethod()
                                .AllowAnyHeader();
                        }
                        else
                        {
                            builder.WithOrigins(corsConfig.GetSection("Origins").ToString().Split(','))
                                .AllowAnyMethod()
                                .AllowAnyHeader();
                        }
                    });
            });


            /*services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });*/
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });


            // JWT
            var jwtOptions = Configuration.GetOptions<JwtOption>("Jwt");

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtOptions.SecretKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL();
            });


            app.UseCommonApplicationBuilder();
        }
    }
}

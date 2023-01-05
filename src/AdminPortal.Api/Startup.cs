using CommonService.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

namespace AdminPortal.Api;

    public class Startup{
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
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
            
            // Auth Services
            services.AddTransient<IPasswordHasher<User>, PasswordHasher<User>>();
            /*services.AddSingleton<IAuthorizationHandler, SkAuthorizationHandler>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("SkAuthorization", policy => policy.Requirements.Add(new SkAuthorizationRequirement()));
            });*/

            services.InjectServicesAndRepositories(Assembly.GetExecutingAssembly());
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


            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });


            //Swagger
            var swaggerConfig = Configuration.GetSection("Swagger");
            if (swaggerConfig.GetValue<bool?>("Enabled").HasValue && swaggerConfig.GetValue<bool?>("Enabled").Value)
            {
                services.AddSwaggerGen(s =>
                {
                    s.SwaggerDoc(swaggerConfig.GetValue<string>("Version"), new OpenApiInfo()
                    {
                        Title = swaggerConfig.GetValue<string>("Title"),
                        Version = swaggerConfig.GetValue<string>("Version")
                    });
                    var jwtSecurityScheme = new OpenApiSecurityScheme
                    {
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                        Name = "JWT Authentication",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                        Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

                        Reference = new OpenApiReference
                        {
                            Id = JwtBearerDefaults.AuthenticationScheme,
                            Type = ReferenceType.SecurityScheme
                        }
                    };
                    s.AddSecurityDefinition("Bearer", jwtSecurityScheme);
                    s.AddSecurityRequirement(new OpenApiSecurityRequirement { { jwtSecurityScheme, Array.Empty<string>() } });
                });
            }

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

            // API
            services.AddApiVersioning();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders();
            app.UseCommonApplicationBuilder();
            // Configure the HTTP request pipeline.
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

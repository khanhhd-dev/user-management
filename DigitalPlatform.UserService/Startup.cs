using DigitalPlatform.UserService.Api.Extensions;
using DigitalPlatform.UserService.Api.Filter;
using DigitalPlatform.UserService.Api.MapperProfile;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.Text;
using DigitalPlatform.UserService.Database;
using DigitalPlatform.UserService.DataAccess.UnitOfWork;
using DigitalPlatform.UserService.Api.Middleware;

namespace DigitalPlatform.UserService.Api
{
    public class Startup
    {
        private readonly string _myAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            try
            {
                //add db
                string dbConnectionString = Configuration.GetConnectionString("DBConnection") ?? string.Empty;
                services.AddDbContext<DatabaseContext>(options =>
                    options.UseMySql(dbConnectionString, ServerVersion.AutoDetect(dbConnectionString))
                        .LogTo(Console.WriteLine, LogLevel.Error));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            // add cors
            services.AddCors(options =>
            {
                options.AddPolicy(_myAllowSpecificOrigins,
                    builder => builder.WithOrigins(Configuration.GetSection("CorsOrigin").Get<string[]>())
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            // Health check
            services.AddHealthChecks();

            // Route Options
            services.AddRouting(options => options.LowercaseUrls = true);

            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromDays(1);
            });

            // auth
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(jwtBearerOptions =>
                {
                    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        ClockSkew = TimeSpan.Zero,
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration.GetValue<string>("JwtConfiguration:Issuer"),
                        ValidAudience = Configuration.GetValue<string>("JwtConfiguration:Audience"),
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetValue<string>("JwtConfiguration:Key")))
                    };
                    jwtBearerOptions.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                context.Response.Headers.Add("Token-Expired", "true");
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddControllers();

            bool isEnableSwagger = Configuration.GetValue<bool>("EnableSwagger");
            if (isEnableSwagger)
            {
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "DigitalPlatform.UserService.Api", Version = "v1" });
                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer"
                    });
                    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },
                                Scheme = "oauth2",
                                Name = "Bearer",
                                In = ParameterLocation.Header,

                            },
                            new List<string>()
                        }
                    });
                });
            }

            services.AddHttpContextAccessor();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddMediatR(AppDomain.CurrentDomain.Load("DigitalPlatform.UserService.Domain"));
            services.AddSingleton(AutoMapperConfig.Initialize());

            services.AddMvc(options =>
            {
                options.Filters.Add(new MyExceptionFilterAttribute());
                options.Filters.Add(new MyActionFilterAttribute());
            })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler("/error");
            bool isEnableSwagger = Configuration.GetValue<bool>("EnableSwagger");

            if (env.IsEnvironment("Local"))
            {
                if (isEnableSwagger)
                {
                    app.UseSwagger();
                    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DigitalPlatform.UserService.Api v1"));
                    app.UseDeveloperExceptionPage();
                }
            }
            else
            {
                app.UseHsts();
                if (isEnableSwagger)
                {
                    app.UseSwagger(c => c.PreSerializeFilters.Add((swaggerDoc, httpRequest) =>
                    {
                        if (!httpRequest.Headers.ContainsKey("X-Forwarded-Host")) return;

                        var serverUrl = $"{httpRequest.Headers["X-Forwarded-Proto"]}://" +
                                        $"{httpRequest.Headers["X-Forwarded-Host"]}/" +
                                        "user-service";

                        swaggerDoc.Servers = new List<OpenApiServer>
                        {
                            new OpenApiServer {Url = serverUrl}
                        };
                    }));
                    app.UseSwaggerUI(c =>
                    {
                        string swaggerJsonBasePath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..";
                        c.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/v1/swagger.json", "DigitalPlatform.UserService.Api v1");
                    });
                }
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(_myAllowSpecificOrigins);

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseMiddleware<PermissionMiddleware>();

            app.UseHealthChecks("/healthcheck");

            app.UseGlobalExceptionHandler();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

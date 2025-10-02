using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using TaskManagement.Domain.Tasks;
using TaskManagement.Domain.UserDetail;
using TaskManagement.EF;
using TaskManagement.EF.DataConfigurations;
using TaskManagement.EF.Repositories;
using TaskManagement.EF.Services;
using TaskManagementWebApi.Filters.Validations;

namespace TaskManagementWebApi
{
    /// <summary>
    /// Startup
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// configuration
        /// </summary>
        public IConfiguration configuration { get; }

        /// <summary>
        /// Startup
        /// </summary>
        /// <param name="_configuration"></param>
        public Startup(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        /// <summary>
        /// Configure Services
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            
           
            
            services.AddDbContext<TaskManagementDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("TaskManagementConnectionString"));
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
            services.AddIdentity<IdentityUser, IdentityRole>(opt =>
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireLowercase = false;
                opt.User.RequireUniqueEmail = true;
                opt.Password.RequiredLength = 1;
            }).AddEntityFrameworkStores<TaskManagementDbContext>()
             .AddDefaultTokenProviders();
            services.AddAutoMapper(x => x.AddProfile(new TaskManagementAutoMapperProfile()));

            AddAuthorization(services);
            services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var result = new ValidationFailedResult(context.ModelState);
                    result.ContentTypes.Add(MediaTypeNames.Application.Json);
                    return result;
                };
            }).AddJsonOptions(x =>x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "TaskManagement API",
                    Description = "TaskManagement Management API"
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            services.AddSignalR();
            services.AddCors();
        }

        /// <summary>
        /// Configure
        /// </summary>
        /// <param name="app">app</param>
        /// <param name="env">env</param>
        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            
            if (env.IsDevelopment())
            {
                IdentityModelEventSource.ShowPII = true;
            }

            app.UseSwagger();
            app.UseSwaggerUI((x) =>
            {
                x.ShowCommonExtensions();
            });

            app.UseCors(builder => builder
                .WithOrigins(configuration["AllowOrigin"])
               .AllowAnyHeader()
               .AllowAnyMethod()
               .SetIsOriginAllowed((host) => true)
               .AllowCredentials()
            );
            UseDatabaseMigrations(app);
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();

            var directoryPath = Path.Combine(env.ContentRootPath, @"Content");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                SeedAdmin.SeedAdminAsync(services).Wait();
            }

            app.MapControllers();
            app.Run();
        }


        private void UseDatabaseMigrations(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<IDbMigrationService>().MigrateAsync().Wait();
            }
        }

        /// <summary>
        /// Add Authorization.
        /// </summary>
        /// <param name="services"></param>
        protected virtual void AddAuthorization(IServiceCollection services)
        {
            services.AddTransient<IUsersRepository, UsersRepository>();
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<ITasksRepository, TaskRepository>();
            services.AddTransient<ITaskService, TaskService>();
            services.AddTransient<IDbMigrationService, TaskManagementDbMigrationService>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = configuration["JWT:ValidAudience"],
                    ValidIssuer = configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"])),
                    ClockSkew = TimeSpan.Zero
                };
                options.Events = new JwtBearerEvents
                {
                    OnForbidden = context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";
                        return context.Response.WriteAsync("{\"error\":\"Unauthorized\"}");
                    }
                };
            });
        }
    }
}








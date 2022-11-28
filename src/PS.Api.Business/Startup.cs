using PS.Application.Interfaces;
using PS.Application.Services;
using PS.Persistence;
using PS.Persistence.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using PS.Persistence.Repositories;
using System.Text.Json.Serialization;
using PS.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Collections.Generic;
using PS.Aplication.Interfaces;
using PS.Aplication.Services;
using PS.Atlassian.Connector.Interfaces;
using PS.Atlassian.Connector;

namespace PS.Api.Business
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
            services.AddDbContext<PSContext>(
                context => context.UseSqlServer(Configuration.GetConnectionString("Default"))
            );

            services.AddIdentityCore<User>(opt =>
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequiredLength = 4;
            })
            .AddRoles<Role>()
            .AddRoleManager<RoleManager<Role>>()
            .AddSignInManager<SignInManager<User>>()
            .AddRoleValidator<RoleValidator<Role>>()
            .AddEntityFrameworkStores<PSContext>()
            .AddDefaultTokenProviders();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(opt =>
                    {
                        opt.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["TokenKey"])),
                            ValidateIssuer = false,
                            ValidateAudience = false
                        };
                    });

    
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IJiraService, JiraService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IStatusService, StatusService>();
            services.AddScoped<IJiraDomainService, JiraDomainService>();
            services.AddScoped<IDashboardService, DashboardService>();


            services.AddScoped<IUserPersist, UserPersist>();
            services.AddScoped<IJiraDomainPersist, JiraDomainPersist>();
            services.AddScoped<IProjectPersist, ProjectPersist>();
            services.AddScoped<ITeamMemberPersist, TeamMemberPersist>();
            services.AddScoped<ISprintPersist, SprintPersist>();
            services.AddScoped<IStatusPersist, StatusPersist>();

            services.AddHttpClient<IAtlassianApiClient, AtlassianApiClient>(client =>
                client.Timeout = TimeSpan.FromSeconds(60)
            );

            services.AddControllers()
                .AddJsonOptions(opt => opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
                .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddCors();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PS.Api.Business", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = @"JWT Authorization header using Bearer.
                                    Enter with 'Bearer ' [space] then put your token
                                    Example: 'Bearer 13g1y2g32y321'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference()
                                        {
                                            Type = ReferenceType.SecurityScheme,
                                            Id = "Bearer"
                                        },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PS.Api.Business v1"));
            }
            app.UseCors(x => x.AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowAnyOrigin());
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

using System;
using System.IO;
using System.Reflection;
using System.Text;
using Chat.Core.Abstract;
using Chat.Core.Hubs;
using Chat.Core.Services;
using Chat.Database.Repository.Manager;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Chat.Extentions
{
    public static class ServiceExtentions
    {
        public static void ConfigureRepositoryManager(this IServiceCollection services)
            => services.AddScoped<IRepositoryManager, RepositoryManager>();

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Chat.API", Version = "v1" });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    BearerFormat = "Bearer {authToken}",
                    Description = "JSON Web Token to access resources. Example: Bearer {token}",
                    Type = SecuritySchemeType.ApiKey
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
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
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });
        }
        
        public static void ConfigureAuthentication(this IServiceCollection services,  IConfiguration configuration)
        {
            var key = Encoding.ASCII.GetBytes(configuration["AppOptions:SecretKey"]);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(x =>
                {
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidateLifetime = false,
                        ValidateIssuer = false,
                        RequireExpirationTime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                    x.SaveToken = true;
                });
        }
        
        public static void ConfigureAuthService(this IServiceCollection services)
            => services.AddScoped<IAuthService, AuthService>();
        
        public static void ConfigureRestoringCodeService(this IServiceCollection services)
            => services.AddScoped<IRestoringCodeService, RestoringCodeServiceService>();
        
        public static void ConfigurePasswordService(this IServiceCollection services)
            => services.AddScoped<IPasswordHasher, PasswordHasherService>();
        
        public static void ConfigureTokenService(this IServiceCollection services)
            => services.AddScoped<ITokenService, TokenService>();
        
        public static void ConfigureCodeService(this IServiceCollection services)
            => services.AddScoped<ICodeService, CodeService>();
        
        public static void ConfigureSmtpService(this IServiceCollection services)
            => services.AddScoped<ISmtpService, SmtpService>();
        
        public static void ConfigureChatService(this IServiceCollection services)
            => services.AddScoped<IChatService, ChatService>();
        
        public static void ConfigureMessageService(this IServiceCollection services)
            => services.AddScoped<IMessageService, MessageService>();
        
        public static void ConfigureUserService(this IServiceCollection services)
            => services.AddScoped<IUserService, UserService>();
        
        public static void ConfigureNotificationService(this IServiceCollection services)
            => services.AddScoped<INotificationService, NotificationService>();
        
        public static void ConfigureChatWatcher(this IServiceCollection services)
            => services.AddScoped<IChatWatcher, ChatWatcher>();
        
    }
}
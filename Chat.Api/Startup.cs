using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using AutoMapper;
using Chat.Common.Error;
using Chat.Common.Exceptions;
using Chat.Core.Auth;
using Chat.Core.Chat;
using Chat.Core.Code;
using Chat.Core.Hashing;
using Chat.Core.Message;
using Chat.Core.Options;
using Chat.Core.ProFiles;
using Chat.Core.Restoring;
using Chat.Core.Services;
using Chat.Core.Smtp;
using Chat.Core.Token;
using Chat.Database;
using Chat.Database.Repository.Chat;
using Chat.Database.Repository.Code;
using Chat.Database.Repository.Manager;
using Chat.Database.Repository.Message;
using Chat.Database.Repository.User;
using Chat.Database.Repository.UserChat;
using Chat.Validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Chat
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
            services.Configure<AppOptions>(Configuration.GetSection(AppOptions.App));
            var appOptions = Configuration.GetSection(AppOptions.App).Get<AppOptions>();
            services.AddSingleton(appOptions);

            ConfigureAuthentication(services);

            ConfigureSwagger(services);

            services.AddScoped<ValidationFilterAttribute>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICodeRepository, CodeRepository>();
            services.AddScoped<IChatRepository, ChatRepository>();
            services.AddScoped<IUserChatRepository, UserChatRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IRepositoryManager, RepositoryManager>();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IRestoringCodeService, RestoringCodeServiceService>();
            services.AddScoped<IPasswordHasher, PasswordHasherService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ISmtpService, SmtpService>();
            services.AddScoped<ICodeService, CodeService>();
            services.AddScoped<IChatService, ChatService>();
            services.AddScoped<IMessageService, MessageService>();

            var con = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AppDbContext>(_ => _.UseNpgsql(con));


            var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new AppProfile()); });
            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

            services.AddControllers(options =>
                options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true).AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);
            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(
                    opt =>
                    {
                        foreach (var description in provider.ApiVersionDescriptions)
                        {
                            opt.SwaggerEndpoint(
                                $"/swagger/{description.GroupName}/swagger.json",
                                description.GroupName.ToUpperInvariant());
                        }
                    }
                );
            }

            ConfigureExceptionHandler(app);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private void ConfigureAuthentication(IServiceCollection services)
        {
            var key = Encoding.ASCII.GetBytes(Configuration["AppOptions:SecretKey"]);

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

        private static void ConfigureSwagger(IServiceCollection services)
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

        private static void ConfigureExceptionHandler(IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.ContentType = "application/json";
                    var contextfeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextfeature != null)
                    {
                        context.Response.StatusCode = contextfeature.Error switch
                        {
                            NotFoundException => StatusCodes.Status404NotFound,
                            BadRequestException => StatusCodes.Status400BadRequest,
                            _ => StatusCodes.Status500InternalServerError
                        };

                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = contextfeature.Error.Message
                        }.ToString());
                    }
                });
            });
        }
    }
}
using System.Text;
using AutoMapper;
using Chat.Core.Abstract;
using Chat.Core.Hubs;
using Chat.Core.Options;
using Chat.Core.ProFiles;
using Chat.Core.Services;
using Chat.Database;
using Chat.Extentions;
using Chat.Validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

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

            services.ConfigureSwagger();

            services.AddScoped<ValidationFilterAttribute>();

            services.ConfigureRepositoryManager();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IRestoringCodeService, RestoringCodeServiceService>();
            services.AddScoped<IPasswordHasher, PasswordHasherService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ISmtpService, SmtpService>();
            services.AddScoped<ICodeService, CodeService>();
            services.AddScoped<IChatService, ChatService>();
            services.AddScoped<IMessageService, MessageService>();
            
            
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IChatWatcher, ChatWatcher>();

            var con = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AppDbContext>(_ => _.UseNpgsql(con));

            var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new AppProfile()); });
            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            
            services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
            });
            
            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

            services.AddControllers(options =>
                options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);
            services.AddHttpContextAccessor();

            services.AddCors(o =>
                o.AddDefaultPolicy(b =>
                    b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
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

            app.ConfigureExceptionHandler();

            app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/chat");
            });
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
    }
}
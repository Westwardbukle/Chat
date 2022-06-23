using System.Text;
using AutoMapper;
using Chat.Core.Abstract;
using Chat.Core.ExternalSources;
using Chat.Core.ExternalSources.Abstract;
using Chat.Core.ExternalSources.Services;
using Chat.Core.Hubs;
using Chat.Core.Options;
using Chat.Core.ProFiles;
using Chat.Core.Services;
using Chat.Database;
using Chat.Extentions;
using Chat.Validation;
using ChatQuartz.Jobs;
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
using Quartz;

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
            
            services.ConfigureAuthentication(Configuration);

            services.ConfigureSwagger();

            services.ConfigureFilters();
            
            services.ConfigureRepositoryManager();
            
            services.ConfigureServices();

            services.ConfigureDbContext(Configuration);
            
            services.ConfigureMapper();
            
            services.AddSignalR(options => { options.EnableDetailedErrors = true; });

            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

            services.AddControllers(options =>
                options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);
            services.AddHttpContextAccessor();

            services.AddScoped<IUserJobService, UserJobService>();
            
            services.ConfigureCors();

            services.AddHttpClient();
            
            services.AddTransient<IUserApi, FakeApi>();
            services.AddTransient<IUserApi, FakerApi>();
            services.AddTransient<IUserApi, DummyJsonApi>();
            
            
            services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionJobFactory();
                
                var jobkey = new JobKey("UsersJob");

                var emailKey = new JobKey("EmailNotifications");

                q.AddJob<UsersJob>(options => options.WithIdentity(jobkey));

                q.AddJob<EmailNotifications>(opt => opt.WithIdentity(emailKey));
                
                q.AddTrigger(options => options
                    .ForJob(jobkey)
                    .WithIdentity("UsersJob-trigger)")
                    .WithCronSchedule("0 0/1 * ? * * *"));
                //0 0/1 * ? * * *
                q.AddTrigger(opt => opt
                    .ForJob(emailKey)
                    .WithIdentity("EmailNotifications-trigger")
                    .WithCronSchedule("0 0 0 ? * * *"));
            });

            services.AddQuartzHostedService(
                q => q.WaitForJobsToComplete = true);
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
    }
}
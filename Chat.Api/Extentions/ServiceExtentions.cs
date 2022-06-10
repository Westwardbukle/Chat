using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
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
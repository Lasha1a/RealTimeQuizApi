using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using RealTimeQuiz.Application.Interfaces.Hubs;
using RealTimeQuiz.Application.Interfaces.JwtToken;
using RealTimeQuiz.Application.Interfaces.PasswordHash;
using RealTimeQuiz.Infrastructure.Security;
using RealTimeQuiz.Infrastructure.Services.Hubs;
using RealTimeQuiz.Infrastructure.Settings;
using System.Text;
using RealTimeQuiz.Application.Interfaces.RedisCache;
using RealTimeQuiz.Infrastructure.Services.RedisCache;

namespace RealTimeQuiz.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureDI(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetSection("Redis")["ConnectionString"];
            options.InstanceName = configuration.GetSection("Redis")["InstanceName"];
        });

        //jwt config register
        services.Configure<JwtSettings>(options =>
            configuration.GetSection("JwtSettings").Bind(options));

        //jwt authentication
        var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings!.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
               Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
            };
        });

        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<ITokenGenerator, TokenGenerator>();
        services.AddScoped<IQuizHubService, QuizHubService>();
        services.AddScoped<ICacheService, CacheService>();

        return services;
    }
}

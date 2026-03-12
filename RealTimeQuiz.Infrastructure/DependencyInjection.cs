using Microsoft.Extensions.DependencyInjection;
using RealTimeQuiz.Application.Interfaces.PasswordHash;
using RealTimeQuiz.Infrastructure.Security;

namespace RealTimeQuiz.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureDI(this IServiceCollection services)
    {

        services.AddScoped<IPasswordHasher, PasswordHasher>();
        return services;
    }
}

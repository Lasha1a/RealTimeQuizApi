using RealTimeQuiz.Application;
using RealTimeQuiz.Infrastructure;
using RealTimeQuiz.Persistence;

namespace RealTimeQuizApi;

public static class DependencyInjection
{
    public static IServiceCollection AddMainApiDI(this IServiceCollection services)
    {
        services.AddInfrastructureDI()
            .AddApplicationDI()
            .AddPersisitenceDi();

        return services;
    }
}

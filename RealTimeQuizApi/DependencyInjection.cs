using RealTimeQuiz.Application;
using RealTimeQuiz.Infrastructure;
using RealTimeQuiz.Persistence;

namespace RealTimeQuizApi;

public static class DependencyInjection
{
    public static IServiceCollection AddMainApiDI(this IServiceCollection services, IConfiguration configurations)
    {
        services.AddInfrastructureDI()
            .AddApplicationDI()
            .AddPersisitenceDi(configurations);

        return services;
    }
}

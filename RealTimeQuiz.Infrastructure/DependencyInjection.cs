using Microsoft.Extensions.DependencyInjection;
using RealTimeQuiz.Application.Interfaces.PasswordHash;
using RealTimeQuiz.Infrastructure.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeQuiz.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureDI(this IServiceCollection services)
    {

        services.AddScoped<IPasswordHasher, PasswordHasher>();
        return services;
    }
}

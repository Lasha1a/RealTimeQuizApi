using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealTimeQuiz.Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeQuiz.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersisitenceDi(this  IServiceCollection services, IConfiguration configurations)
    {
        services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(configurations.GetConnectionString("PostgreSQL")));

        return services;
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealTimeQuiz.Application.Interfaces.GenericRepo;
using RealTimeQuiz.Persistence.Data;
using RealTimeQuiz.Persistence.Repositories;
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
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true); //fixes utcdatetime unspecified error in postgresql

        services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(configurations.GetConnectionString("PostgreSQL")));

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        return services;
    }
}

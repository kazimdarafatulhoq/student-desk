using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudentManagement.Application.Interfaces;
using StudentManagement.Application.Services;
using StudentManagement.Infrastructure.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace StudentManagement.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("StudentManagementDb")
                ?? configuration["ConnectionStrings:StudentManagementDb"]
                ?? "Server=localhost;Database=StudentManagementDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true";

            services.AddDbContext<StudentManagementDbContext>(options =>
                options.UseSqlServer(connectionString, sql =>
                {
                    sql.EnableRetryOnFailure(3);
                    sql.CommandTimeout(60);
                }));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<StudentService>();
            services.AddScoped<FeeService>();
            services.AddScoped<AuthService>();
            services.AddScoped<ExamService>();

            return services;
        }
    }
}

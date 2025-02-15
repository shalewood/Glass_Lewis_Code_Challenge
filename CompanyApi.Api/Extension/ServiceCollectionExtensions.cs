using CompanyApi.Api.Data;
using CompanyApi.Api.Repository.Implementation;
using CompanyApi.Api.Repository.Interface;
using CompanyApi.Api.Services.Implementation;
using CompanyApi.Api.Services.Interface;
using CompanyApi.Api.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CompanyApi.Api.Extension
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCompanyDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDBContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            return services;
        }

        public static IServiceCollection AddService(this IServiceCollection services)
        {
            services.AddScoped<ICompanyService, CompanyService>();
            return services;
        }
        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            return services;
        }

        public static IServiceCollection AddFluentValidation(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<CompanyDTOValidator>();
            return services;
        }
    }
}

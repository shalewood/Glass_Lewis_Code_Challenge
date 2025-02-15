
using CompanyApi.Api.Extension;

namespace CompanyApi.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // Add Automapper config Settings
            builder.Services.AddAutoMapper(typeof(MappingConfig));
            //Setting Up To Register Enity Framework
            builder.Services.AddCompanyDbContext(builder.Configuration);
            // Add Repository
            builder.Services.AddService();
            // Add Service
            builder.Services.AddRepository();
            // Add FluentValidation
            builder.Services.AddFluentValidation();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

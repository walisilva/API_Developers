using API_Developers.Business;
using API_Developers.Business.Implementations;
using API_Developers.Model.Context;
using API_Developers.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace API_Developers
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // Banco de dados
            string? connection = Configuration["MySQLConnection:MySQLConnectionString"];
            services.AddDbContext<MySQLContext>(options => options.UseMySql(connection, new MySqlServerVersion(new Version(8, 0, 31))));

            // Dependency Injection
            services.AddScoped<IDeveloperBusiness, DeveloperBusinessImplementation>();

            // Generic Repository
            services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

            // Dependency Injection - End

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        public void Configure(WebApplication app, IWebHostEnvironment environment)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();
        }
    }
}

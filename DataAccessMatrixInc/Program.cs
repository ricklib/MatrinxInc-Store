using DataAccessLayer;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DataAccessMatrixInc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<MatrixIncDbContext>();
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            else
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }

            using(var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var context = services.GetRequiredService<MatrixIncDbContext>();
                context.Database.EnsureCreated();
                MatrixIncDbInitializer.Initialize(context);
            }

            app.MapGet("/", () => "Hello World!");
            app.MapGet("/customers", (MatrixIncDbContext context) =>
            {
                var repo = new CustomerRepository(context);
                var customers = repo.GetAllCustomers();
                return customers;
            });
            app.Run();
        }
    }
}

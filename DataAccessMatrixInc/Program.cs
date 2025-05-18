using DataAccessLayer;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace DataAccessMatrixInc;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // We gebruiken voor nu even een SQLite voor de database,
        // omdat deze eenvoudig lokaal te gebruiken is en geen extra configuratie nodig heeft.
        builder.Services.AddDbContext<MatrixIncDbContext>(
            options => options.UseSqlite("Data Source=MatrixInc.db"));

        // voor development doeleinden, zodat we de database kunnen bekijken
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        // We registreren de repositories in de DI container
        builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
        builder.Services.AddScoped<IOrderRepository, OrderRepository>();
        builder.Services.AddScoped<IProductRepository, ProductRepository>();

        // Prevents serialization from throwing an exception when it encounters circular references and set pretty print for debugging
        builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(
            options => { 
                options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; 
                options.SerializerOptions.WriteIndented = true; 
            });            

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

        // Hele simpele overzichtelijke routing als voorbeeld
        //
        // Dit mag naar eigen inzicht worden aangepast
        //
        app.MapGet("/html", () => Results.Extensions.Html(@$"<!doctype html>
                    <html>
                        <head><title>Overview 'get all' provided methods for: customers, orders, products, parts</title></head>
                        <body>
                            <h1>Methods:</h1>                                
                            <p>Click <a href=""/customers"">here</a> to get all customers</p>
                            <p>Click <a href=""/orders"">here</a> to get all orders</p>
                            <p>Click <a href=""/products"">here</a> to get all products</p>
                            <p>Click <a href=""/parts"">here</a> to get all parts</p>
                        </body>
                    </html>"));
        app.MapGet("/", () => "Hello World! please browse to /html ");
        app.MapGet("/customers", (MatrixIncDbContext context) =>
        {
            var repo = new CustomerRepository(context);
            var customers = repo.GetAllCustomers().ToList();
            return customers;
        });
        app.MapGet("/orders", (MatrixIncDbContext context) =>
        {
            var repo = new OrderRepository(context);
            var orders = repo.GetAllOrders().ToList();
            return orders;
        });
        app.MapGet("/products", (MatrixIncDbContext context) =>
        {
            var repo = new ProductRepository(context);
            var products = repo.GetAllProducts().ToList();
            return products;
        });
        app.MapGet("/parts", (MatrixIncDbContext context) =>
        {
            var repo = new PartRepository(context);
            var parts = repo.GetAllParts().ToList();
            return parts;
        });

        app.Run();
    }
}

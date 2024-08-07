using DataAccessLayer;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Net.Mime;
using System.Text;
using System.Text.Json.Serialization;

static class ResultsExtensions
{
    public static IResult Html(this IResultExtensions resultExtensions, string html)
    {
        ArgumentNullException.ThrowIfNull(resultExtensions);

        return new HtmlResult(html);
    }
}

class HtmlResult : IResult
{
    private readonly string _html;

    public HtmlResult(string html)
    {
        _html = html;
    }

    public Task ExecuteAsync(HttpContext httpContext)
    {
        httpContext.Response.ContentType = MediaTypeNames.Text.Html;
        httpContext.Response.ContentLength = Encoding.UTF8.GetByteCount(_html);
        return httpContext.Response.WriteAsync(_html);
    }
}

namespace DataAccessMatrixInc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<MatrixIncDbContext>(
                options => options.UseSqlite("Data Source=MatrixInc.db"));

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();

            // Prevents serialization from throwing an exception when it encounters circular references
            builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

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
    <head><title>miniHTML</title></head>
    <body>
        <h1>Hello World</h1>
        <p>The time on the server is {DateTime.Now:O}</p>
    </body>
</html>"));
            app.MapGet("/", () => "Hello World!");
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

            app.Run();
        }
    }
}

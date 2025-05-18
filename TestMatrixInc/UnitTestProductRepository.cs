using DataAccessLayer;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMatrixInc;

public class UnitTestProductRepository
{
    private ServiceProvider _serviceProvider;

    [SetUp]
    public void Setup()
    {
        var services = new ServiceCollection();

        // Using In-Memory database for testing
        services.AddDbContext<MatrixIncDbContext>(options => options.UseInMemoryDatabase("MatrixIncTestDb").EnableSensitiveDataLogging());
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();

        _serviceProvider = services.BuildServiceProvider();
    }

    [TearDown]
    public void TearDown()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<MatrixIncDbContext>();
            context.Database.EnsureDeleted();
        }
    }

    [Test]
    public void TestCreateProduct()
    {
        // arrange
        using (var scope = _serviceProvider.CreateScope())
        {
            var services = scope.ServiceProvider;
            var repository = services.GetRequiredService<IProductRepository>();

            // act
            Product product = new Product
            {
                Name = "Widget",
                Price = 10.00m,
                Description = "A widget",
                Image = new byte[] { 0xFF, 0xFF, 0xFF } // Dummy image data
            };
            repository.AddProduct(product);

            // assert
            var savedProduct = repository.GetProductById(product.Id);
            Assert.IsNotNull(savedProduct);
            Assert.That(savedProduct.Name, Is.EqualTo(product.Name));
        }
    }
}


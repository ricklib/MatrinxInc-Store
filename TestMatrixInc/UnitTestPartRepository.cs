using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Models;

namespace TestMatrixInc;

public class UnitTestPartRepository
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
        services.AddScoped<IPartRepository, PartRepository>();

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
    public void TestAddPart()
    {
        // arrange
        using (var scope = _serviceProvider.CreateScope())
        {
            var services = scope.ServiceProvider;                
            var partRepository = services.GetRequiredService<IPartRepository>();

            // act
            Part part = new Part
            {
                Name = "Generic Part",                    
                Description = "A generic part for the widget",
                Image = new byte[] { 0xFF, 0xFF, 0xFF } // Dummy image data
            };
            partRepository.AddPart(part);

            // assert
            var savedPart = partRepository.GetPartById(part.Id);
            Assert.IsNotNull(savedPart);
            Assert.That(savedPart.Name, Is.EqualTo(part.Name));
        }
    }   
}

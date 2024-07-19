using DataAccessLayer;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace TestMatrixInc
{
    public class Tests
    {
        private ServiceProvider _serviceProvider;

        [SetUp]
        public void Setup()
        {
            var services = new ServiceCollection();

            // Using In-Memory database for testing
            services.AddDbContext<MatrixIncDbContext>(options => options.UseInMemoryDatabase("MatrixIncTestDb"));
            services.AddScoped<ICustomerRepository, CustomerRepository>();            

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
        public void TestCreateCustomer()
        {
            // arrange
            using(var scope = _serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var repository = services.GetRequiredService<ICustomerRepository>();

                // act
                Customer customer = new Customer
                {
                    Name = "John Doe",
                    Address = "123 Main",
                    Active = true
                };
                repository.AddCustomer(customer);

                // assert
                var savedCustomer = repository.GetCustomerById(customer.Id);
                Assert.IsNotNull(savedCustomer);
                Assert.That(savedCustomer.Name, Is.EqualTo(customer.Name));
            }
        }
    }
}
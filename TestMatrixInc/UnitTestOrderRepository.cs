using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;
using DataAccessLayer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Models;

namespace TestMatrixInc
{
    public class OrderRepositoryTests
    {
        private ServiceProvider _serviceProvider;

        [SetUp]
        public void Setup()
        {
            var services = new ServiceCollection();

            // Using In-Memory database for testing
            services.AddDbContext<MatrixIncDbContext>(options => options.UseInMemoryDatabase("MatrixIncTestDb"));
            services.AddScoped<IOrderRepository, OrderRepository>();

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
        public void TestCreateOrderForCustomer()
        {
            // arrange
            using (var scope = _serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var repository = services.GetRequiredService<IOrderRepository>();

                // act
                Customer customer = new Customer
                {
                    Name = "John Doe",
                    Address = "123 Main",
                    Active = true
                };
                Order order = new Order
                {
                    Customer = customer,
                    OrderDate = DateTime.Now,                    
                };
                repository.AddOrder(order);

                // assert
                var savedOrder = repository.GetOrderById(order.Id);
                Assert.IsNotNull(savedOrder);
                Assert.That(savedOrder.Id, Is.EqualTo(order.Id));
                Assert.That(savedOrder.Customer.Id, Is.EqualTo(customer.Id));
            }
        }
    }
}
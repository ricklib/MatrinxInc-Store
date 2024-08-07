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
            services.AddDbContext<MatrixIncDbContext>(options => options.UseInMemoryDatabase("MatrixIncTestDb").EnableSensitiveDataLogging());                             
            services.AddScoped<IOrderRepository, OrderRepository>();
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

        [Test]
        public void TestDeleteOrderForCustomer()
        {
            // arrange
            using (var scope = _serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var orderRepository = services.GetRequiredService<IOrderRepository>();

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
                orderRepository.AddOrder(order);

                // assert
                var savedOrder = orderRepository.GetOrderById(order.Id);
                Assert.IsNotNull(savedOrder);
                Assert.That(savedOrder.Id, Is.EqualTo(order.Id));
                Assert.That(savedOrder.Customer.Id, Is.EqualTo(customer.Id));

                // act
                orderRepository.DeleteOrder(order);

                // assert
                var deletedOrder = orderRepository.GetOrderById(order.Id);
                Assert.IsNull(deletedOrder);

                // I want to make sure that the customer is still in the database
                var customerRepository = services.GetRequiredService<ICustomerRepository>();
                var nonDeletedCustomer = customerRepository.GetCustomerById(customer.Id);
                Assert.IsNotNull(nonDeletedCustomer);
            }
        }

        [Test]
        public void TestUpdateOrderForCustomer()
        {
            // arrange
            using (var scope = _serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var orderRepository = services.GetRequiredService<IOrderRepository>();

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
                orderRepository.AddOrder(order);

                // assert
                var savedOrder = orderRepository.GetOrderById(order.Id);
                Assert.IsNotNull(savedOrder);
                Assert.That(savedOrder.Id, Is.EqualTo(order.Id));
                Assert.That(savedOrder.Customer.Id, Is.EqualTo(customer.Id));

                // act
                savedOrder.OrderDate = DateTime.Now.AddDays(1);
                orderRepository.UpdateOrder(savedOrder);

                // assert
                var updatedOrder = orderRepository.GetOrderById(order.Id);
                Assert.IsNotNull(updatedOrder);
                Assert.That(updatedOrder.Id, Is.EqualTo(savedOrder.Id));
                Assert.That(updatedOrder.Customer.Id, Is.EqualTo(customer.Id));
                Assert.That(updatedOrder.OrderDate, Is.EqualTo(savedOrder.OrderDate));
            }
        }
    }
}
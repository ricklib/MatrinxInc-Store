using DataAccessLayer;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace TestMatrixInc
{
    public class CustomerRepositoryTests
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

        [Test]
        public void TestUpdateCustomer()
        {
            // arrange
            using(var scope = _serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var repository = services.GetRequiredService<ICustomerRepository>();

                Customer customer = new Customer
                {
                    Name = "John Doe",
                    Address = "123 Main",
                    Active = true
                };
                repository.AddCustomer(customer);

                // act
                customer.Name = "Jane Doe";
                repository.UpdateCustomer(customer);

                // assert
                var updatedCustomer = repository.GetCustomerById(customer.Id);
                Assert.IsNotNull(updatedCustomer);
                Assert.That(updatedCustomer.Name, Is.EqualTo("Jane Doe"));
            }
        }

        [Test]
        public void TestDeleteCustomer()
        {
            // arrange
            using(var scope = _serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var repository = services.GetRequiredService<ICustomerRepository>();

                Customer customer = new Customer
                {
                    Name = "John Doe",
                    Address = "123 Main",
                    Active = true
                };
                repository.AddCustomer(customer);

                // act
                repository.DeleteCustomer(customer);

                // assert
                var deletedCustomer = repository.GetCustomerById(customer.Id);
                Assert.IsNull(deletedCustomer);
            }
        }

        [Test]
        public void TestGetAllCustomers()
        {
            // arrange
            using(var scope = _serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var repository = services.GetRequiredService<ICustomerRepository>();

                Customer customer1 = new Customer
                {
                    Name = "John Doe",
                    Address = "123 Main",
                    Active = true
                };
                repository.AddCustomer(customer1);

                Customer customer2 = new Customer
                {
                    Name = "Jane Doe",
                    Address = "456 Main",
                    Active = true
                };
                repository.AddCustomer(customer2);

                // act
                var customers = repository.GetAllCustomers();

                // assert
                Assert.That(customers.Count(), Is.EqualTo(2));
            }
        }

        [Test]
        public void TestCreateCustomerWithOrder()
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
                customer.Orders.Add(new Order { OrderDate = DateTime.Now });
                repository.AddCustomer(customer);

                // assert
                var savedCustomer = repository.GetCustomerById(customer.Id);
                Assert.IsNotNull(savedCustomer);
                Assert.That(savedCustomer.Orders.Count, Is.EqualTo(1));
            }
        }
    }
}
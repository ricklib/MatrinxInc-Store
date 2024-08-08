using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public static class MatrixIncDbInitializer
    {
        public static void Initialize(MatrixIncDbContext context)
        {
            // Look for any customers.
            if (context.Customers.Any())
            {
                return;   // DB has been seeded
            }

            // TODO: Hier moet ik nog wat namen verzinnen die betrekking hebben op de matrix.
            // - Denk aan de m3 boutjes, moertjes en ringetjes.
            // - Denk aan namen van schepen
            // - Denk aan namen van vliegtuigen
            // - Denk aan namen van personages uit de matrix zoals trinity, neo, morpheus, agent smith, etc.

            var customers = new Customer[]
            {
                new Customer { Name = "John Doe", Address = "123 Elm St" , Active=true},
                new Customer { Name = "Jane Smith", Address = "456 Oak St", Active = true },
                new Customer { Name = "Joe Blow", Address = "789 Pine St", Active = true }
            };
            context.Customers.AddRange(customers);

            var orders = new Order[]
            {
                new Order { Customer = customers[0], OrderDate = DateTime.Parse("2021-01-01")},
                new Order { Customer = customers[0], OrderDate = DateTime.Parse("2021-02-01")},
                new Order { Customer = customers[1], OrderDate = DateTime.Parse("2021-02-01")},
                new Order { Customer = customers[2], OrderDate = DateTime.Parse("2021-03-01")}
            };  
            context.Orders.AddRange(orders);

            var products = new Product[]
            {
                new Product { Name = "Widget", Description = "Description Widget", Price = 9.99m },
                new Product { Name = "Gadget", Description = "Description Gadget", Price = 19.99m },
                new Product { Name = "Doohickey", Description = "Description Doohickey", Price = 29.99m }
            };
            context.Products.AddRange(products);

            context.SaveChanges();

            context.Database.EnsureCreated();
        }
    }
}

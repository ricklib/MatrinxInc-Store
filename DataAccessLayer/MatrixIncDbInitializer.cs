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

            var customers = new Customer[]
            {
                new Customer { Name = "John Doe", Address = "123 Elm St" },
                new Customer { Name = "Jane Smith", Address = "456 Oak St" },
                new Customer { Name = "Joe Blow", Address = "789 Pine St" }
            };
            context.Customers.AddRange(customers);
            context.SaveChanges();

            context.Database.EnsureCreated();
        }
    }
}

using CRM.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Models
{
    public class DbInitializer
    {
        public static void Initialize(CRMContext context)
        {
            context.Database.EnsureCreated();

            // Check if the Clients table is empty
            if (!context.Clients.Any())
            {
                // Create some sample clients
                var clients = new List<Client>
                {
                    new Client { Name = "John Doe", Passport = "1234567890", Contacts = "john.doe@example.com" },
                    new Client { Name = "Jane Smith", Passport = "0987654321", Contacts = "jane.smith@example.com" },
                    new Client { Name = "Bob Johnson", Passport = "4567890123", Contacts = "bob.johnson@example.com" },
                };

                // Add the clients to the database
                context.Clients.AddRange(clients);
                context.SaveChanges();
            }

            // Check if the Products table is empty
            if (!context.Products.Any())
            {
                // Create some sample products
                var products = new List<Product>
                {
                    new Product { Name = "Product 1", Description = "Description 1", Price = 100.0m },
                    new Product { Name = "Product 2", Description = "Description 2", Price = 200.0m },
                    new Product { Name = "Product 3", Description = "Description 3", Price = 300.0m },
                };

                // Add the products to the database
                context.Products.AddRange(products);
                context.SaveChanges();
            }

            // Check if the Orders table is empty
            if (!context.Orders.Any())
            {
                // Create some sample orders
                var orders = new List<Order>
                {
                    new Order
                {
                    OrderNumber = 1001,
                    Date = DateTime.Now,
                    Client = context.Clients.ToList()[0],
                    OrderItems = new List<OrderItem>
                    {
                        new OrderItem { Product = context.Products.ToList()[0], Quantity = 2 },
                        new OrderItem { Product = context.Products.ToList()[1], Quantity = 1 }
                    }
                },
                new Order
                {
                    OrderNumber = 1002,
                    Date = DateTime.Now,
                    Client = context.Clients.ToList()[2],
                    OrderItems = new List<OrderItem>
                    {
                        new OrderItem { Product = context.Products.ToList()[1], Quantity = 3 },
                        new OrderItem { Product = context.Products.ToList()[2], Quantity = 1 }
                    }
                },
                };

                // Add the orders to the database
                context.Orders.AddRange(orders);
                context.SaveChanges();
            }
        }
    }
}

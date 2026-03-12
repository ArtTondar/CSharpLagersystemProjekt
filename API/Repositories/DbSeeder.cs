using API.Models;
using Bogus;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class DbSeeder
    {
        public DbSeeder()
        {
            var faker = new Faker();

            // ---------- 1. Generate Products ----------
            var productFaker = new Faker<Product>()
                  .RuleFor(p => p.Id, f => Guid.NewGuid())
                .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                .RuleFor(p => p.UnitPrice, f => f.Finance.Amount(5, 500))
                .RuleFor(p => p.UnitStatus, f => f.PickRandom<UnitStatus>())
                .RuleFor(p => p.Size, f => f.Random.Int(1, 100))
                .RuleFor(p => p.UnitStock, f => f.Random.Int(1, 5))
                .RuleFor(p => p.Description, f => f.Lorem.Sentence(10));

            var products = productFaker.Generate(20);

            var availableProducts = products
                .Where(p => p.UnitStatus == UnitStatus.InStock)
                .ToList();

            // ---------- 2. Generate Customers ----------
            //no danish locale >:(
            var germanFaker = new Faker("de");

            var customerFaker = new Faker<Customer>()
                  .RuleFor(c => c.Id, f => Guid.NewGuid())
                .RuleFor(c => c.Name, f => f.Person.FullName)
                .RuleFor(c => c.Email, f => f.Person.Email)
                .RuleFor(c => c.Phone, f => f.Person.Phone)
                .RuleFor(c => c.City, f => germanFaker.Address.City())           
                .RuleFor(c => c.Street, f => germanFaker.Address.StreetAddress())
                .RuleFor(c => c.ZipCode, f => germanFaker.Address.ZipCode());

            var customers = customerFaker.Generate(50);

            // ---------- 3. Generate Orders and OrderDetails ----------
            var orderFaker = new Faker<Order>()
                  .RuleFor(o => o.Id, f => Guid.NewGuid())
                .RuleFor(o => o.OrderDate, f => f.Date.Past(1));

            var orderLineFaker = new Faker<OrderDetail>()
                  .RuleFor(ol => ol.Id, f => Guid.NewGuid())
                .RuleFor(ol => ol.Quantity, f => f.Random.Int(1, 10))
                .RuleFor(ol => ol.UnitPrice, f => 0m); // will overwrite with product price

            var orders = new List<Order>();
            var orderLines = new List<OrderDetail>();

            foreach (var customer in customers)
            {
                // Generate 1–5 orders per customer
                var customerOrders = orderFaker.Generate(faker.Random.Int(1, 5));

                foreach (var order in customerOrders)
                {
                    order.CustomerId = customer.Id; // assign FK
                    order.OrderDetails = new List<OrderDetail>();

                    // Generate 1–5 order lines per order
                    var lines = orderLineFaker.Generate(faker.Random.Int(1, 5));

                    foreach (var line in lines)
                    {
                        if (!availableProducts.Any()) break;

                        var product = faker.PickRandom(availableProducts);
                        line.ProductId = product.Id;  // assign FK
                        line.UnitPrice = product.UnitPrice;
                        line.OrderId = order.Id;      // assign FK

                        order.OrderDetails.Add(line);
                        orderLines.Add(line);
                    }

                    orders.Add(order);
                    customer.Orders.Add(order);
                }
            }

            var userFaker = new Faker<User>()
                .RuleFor(u => u.Id, f => Guid.NewGuid())
                .RuleFor(u => u.Name, f => f.Person.FullName)
                .RuleFor(u => u.Email, f => f.Person.Email)
                .RuleFor(u => u.Password, f => f.Internet.Password()) // plain text for demo purposes
                .RuleFor(u => u.Phone, f => f.Person.Phone)
                .RuleFor(u => u.IsAdmin, f => f.Random.Bool(0.3f)); // ~30% admins, 70% regular

            var users = userFaker.Generate(20); // generate 20 users

            // ---------- 4. Insert into database ----------
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(
                "Data Source=localhost;Database=LagerSystemDatabase;Persist Security Info=False;User ID=sa;Password=Sup3rPassword!;TrustServerCertificate=True;");

            using (var context = new AppDbContext(optionsBuilder.Options))
            {
                context.BulkInsert(products);
                context.BulkInsert(customers);
                context.BulkInsert(orders);
                context.BulkInsert(orderLines);
            }
        }
    }
}
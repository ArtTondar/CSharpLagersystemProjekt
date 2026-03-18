using API.Models;
using API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _dbContext;

        public OrderRepository(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }

        public async Task<Order> Create(Order order)
        {
            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();

            return order;
        }

        public async Task Delete(Order order)
        {
            _dbContext.Remove(order);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Order>> GetAll()
        {
            return await _dbContext.Orders.AsNoTracking().ToListAsync();
        }

        public async Task<List<Order>> GetByDate(DateTime start, DateTime? end = null)
        {
            if (end == null)
            {
                return await _dbContext.Orders
                    .Where(o => o.OrderDate >= start)
                    .AsNoTracking()
                    .ToListAsync();
            }

            return await _dbContext.Orders
                .Where(o => o.OrderDate >= start && o.OrderDate <= end)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Order?> GetById(Guid id)
        {
            // Ændret:
            // Include(order => order.OrderDetails) er tilføjet.
            //
            // Hvorfor:
            // UI og update-flow skal hente ordren sammen med ordrelinjerne.
            return await _dbContext.Orders
                .Include(order => order.OrderDetails)
                .AsNoTracking()
                .FirstOrDefaultAsync(order => order.Id == id);
        }

        public async Task<List<Order>> GetByTotalPrice(decimal totalPrice)
        {
            return await _dbContext.Orders
                .Where(o => o.TotalPrice == totalPrice)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Order>> GetByCustomerId(Guid customerId)
        {
            return await _dbContext.Orders
                .Where(o => o.CustomerId == customerId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task Update(Order updatedOrder)
        {
            // Ændret:
            // Nye OrderDetails bliver nu tilføjet direkte via _dbContext.OrderDetails.AddAsync(...)
            // i stedet for existingOrder.OrderDetails.Add(...).
            //
            // Hvorfor:
            // Når en ny OrderDetail allerede har fået et Guid i klienten,
            // kan EF i nogle tilfælde forsøge at behandle den som en eksisterende række
            // og lave UPDATE i stedet for INSERT.
            // Det gav concurrency-fejlen:
            // "expected to affect 1 row(s), but actually affected 0 row(s)".
            //
            // Ved at tilføje nye OrderDetails eksplicit til DbSet'et,
            // bliver de markeret som Added og gemt korrekt som INSERT.

            ArgumentNullException.ThrowIfNull(updatedOrder);

            List<OrderDetail> updatedOrderDetails = updatedOrder.OrderDetails ?? new List<OrderDetail>();

            Order? existingOrder = await _dbContext.Orders
                .Include(order => order.OrderDetails)
                .FirstOrDefaultAsync(order => order.Id == updatedOrder.Id);

            if (existingOrder is null)
            {
                throw new InvalidOperationException("Ordren blev ikke fundet.");
            }

            existingOrder.CustomerId = updatedOrder.CustomerId;
            existingOrder.OrderDate = updatedOrder.OrderDate;
            existingOrder.TotalPrice = updatedOrder.TotalPrice;

            List<OrderDetail> detailsToRemove = existingOrder.OrderDetails
                .Where(existingDetail => updatedOrderDetails
                    .All(updatedDetail => updatedDetail.Id != existingDetail.Id))
                .ToList();

            foreach (OrderDetail detailToRemove in detailsToRemove)
            {
                _dbContext.OrderDetails.Remove(detailToRemove);
            }

            foreach (OrderDetail existingDetail in existingOrder.OrderDetails)
            {
                OrderDetail? updatedDetail = updatedOrderDetails
                    .FirstOrDefault(detail => detail.Id == existingDetail.Id);

                if (updatedDetail is null)
                {
                    continue;
                }

                existingDetail.ProductId = updatedDetail.ProductId;
                existingDetail.Quantity = updatedDetail.Quantity;
                existingDetail.UnitPrice = updatedDetail.UnitPrice;
            }

            List<OrderDetail> newDetails = updatedOrderDetails
                .Where(updatedDetail => existingOrder.OrderDetails
                    .All(existingDetail => existingDetail.Id != updatedDetail.Id))
                .Select(updatedDetail => new OrderDetail
                {
                    Id = updatedDetail.Id,
                    OrderId = existingOrder.Id,
                    ProductId = updatedDetail.ProductId,
                    Quantity = updatedDetail.Quantity,
                    UnitPrice = updatedDetail.UnitPrice
                })
                .ToList();

            foreach (OrderDetail newDetail in newDetails)
            {
                await _dbContext.OrderDetails.AddAsync(newDetail);
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
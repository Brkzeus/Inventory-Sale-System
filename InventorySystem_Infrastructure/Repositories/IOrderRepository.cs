using InventorySystem_Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem_Infrastructure.Repositories
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        // Custom query to fetch an order with its OrderItems and Product details (SQL JOIN)
        Task<Order?> GetOrderWithDetailsAsync(int orderId);

        // Custom query to fetch all orders for a specific user
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId);
    }
}

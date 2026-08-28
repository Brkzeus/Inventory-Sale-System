using InventorySystem_Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventorySystem_Infrastructure;

namespace InventorySystem_Infrastructure.Repositories
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IProductRepository Products { get; }
        IOrderRepository Orders { get; }

        // Purely generic repository for OrderItems (if needed directly)
        IGenericRepository<OrderItem> OrderItems { get; }

        IGenericRepository<Category> Categories { get; }

        Task<int> CompleteAsync();
    }
}

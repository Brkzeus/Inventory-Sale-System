using InventorySystem_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem_Infrastructure.Repositories
{
    public interface IGenericRepository<T> where T :class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        IQueryable<T> Find(Expression<Func<T, bool>> expression);

        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}

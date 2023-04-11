using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Core.ErrorHandling;
using UserManagement.Core.Models;

namespace UserManagement.Core.Services
{
    public interface IService<T> where T : class
    {
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task RemoveAsync(T entity);

        Task<Result<T>> GetByIdAsync(int id);
        Task<Result<IEnumerable<T>>> GetAllAsync();
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Core.Models;
using UserManagement.Core.Repositories;

namespace UserManagement.Data.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<User> GetByUserNameAndPasswordAsync(string userName, string password)
        {
            return await _dbSet.SingleOrDefaultAsync(u => u.UserName == userName && u.Password == password);
        }

        public async Task<User> GetByUserNameAsync(string userName)
        {
            return await _dbSet.SingleOrDefaultAsync(u => u.UserName == userName);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Core.Repositories;

namespace UserManagement.Core.UnitOfWork
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }

        void Commit();
        void Rollback();
        Task CommitAsync();
        Task RollbackAsync();
    }
}

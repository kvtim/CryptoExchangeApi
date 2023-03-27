using CurrencyManagement.Core.Repositories;
using CurrencyManagement.Core.UnitOfWork;
using CurrencyManagement.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyManagement.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private ICurrencyRepository _currencyRepository;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public ICurrencyRepository CurrencyRepository => _currencyRepository =
            _currencyRepository ?? new CurrencyRepository(_context);

        public void Commit()
        {
            _context.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Rollback()
        {
            _context.Dispose();
        }

        public async Task RollbackAsync()
        {
            await _context.DisposeAsync();
        }
    }
}

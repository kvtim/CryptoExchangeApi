using CurrencyManagement.Core.Models;
using CurrencyManagement.Core.UnitOfWork;
using CurrencyManagement.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyManagement.Services.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CurrencyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Currency> AddAsync(Currency entity)
        {
            await _unitOfWork.CurrencyRepository.AddAsync(entity);
            await _unitOfWork.CommitAsync();

            return entity;
        }

        public async Task<IEnumerable<Currency>> GetAllAsync()
        {
            return await _unitOfWork.CurrencyRepository.GetAllAsync();
        }

        public async Task<Currency> GetByIdAsync(int id)
        {
            return await _unitOfWork.CurrencyRepository.GetByIdAsync(id);
        }

        public async Task RemoveAsync(Currency entity)
        {
            await _unitOfWork.CurrencyRepository.RemoveAsync(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task<Currency> UpdateAsync(Currency entity)
        {
            await _unitOfWork.CurrencyRepository.UpdateAsync(entity);
            await _unitOfWork.CommitAsync();

            return entity;
        }
    }
}

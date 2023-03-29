using CurrencyManagement.Core.Models;
using CurrencyManagement.Core.UnitOfWork;
using CurrencyManagement.Core.Services;
using CurrencyManagement.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            entity.CurrencyDimensions = new List<CurrencyDimension>
            {
                new CurrencyDimension()
                {
                    PriceInUSD = entity.CurrentPriceInUSD,
                }
            };
            await _unitOfWork.CurrencyRepository.AddAsync(entity);
            await _unitOfWork.CommitAsync();

            return entity;
        }

        public async Task<IEnumerable<Currency>> GetAllAsync()
        {
            return await _unitOfWork.CurrencyRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Currency>> GetAllWithDimensionAsync()
        {
            return await _unitOfWork.CurrencyRepository.GetAllWithDimensionAsync();
        }

        public async Task<Currency> GetByIdAsync(int id)
        {
            return await _unitOfWork.CurrencyRepository.GetByIdAsync(id);
        }

        public async Task<Currency> GetByIdlWithDimensionAsync(int id)
        {
            return await _unitOfWork.CurrencyRepository.GetByIdWithDimensionAsync(id);
        }

        public async Task RemoveAsync(Currency entity)
        {
            await _unitOfWork.CurrencyRepository.RemoveAsync(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task<Currency> UpdateAsync(Currency entity)
        {
            await _unitOfWork.CurrencyRepository.UpdateAsync(entity);

            await _AddDimension(entity);

            await _unitOfWork.CommitAsync();

            return entity;
        }

        public async Task<Currency> UpdatePriceAsync(int id, decimal increasePrice)
        {
            var currency = await GetByIdAsync(id);

            if (currency == null || currency.CurrentPriceInUSD <= -increasePrice)
                return null;

            currency.CurrentPriceInUSD += increasePrice;

            return await UpdateAsync(currency);
        }

        private async Task _AddDimension(Currency entity)
        {
            var updatedEntity = await _unitOfWork.CurrencyRepository
                .GetByIdWithDimensionAsync(entity.Id);

            var lastDim = updatedEntity.CurrencyDimensions.First(c => c.IsCurrent == true);
            lastDim.EndDate = DateTime.UtcNow;
            lastDim.IsCurrent = false;

            updatedEntity.CurrencyDimensions.Add(
                new CurrencyDimension()
                {
                    FromDate = DateTime.UtcNow,
                    PriceInUSD = entity.CurrentPriceInUSD
                });
        }
    }
}

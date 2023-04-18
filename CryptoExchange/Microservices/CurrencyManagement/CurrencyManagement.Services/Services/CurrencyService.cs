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
using CurrencyManagement.Data.Exceptions;
using CurrencyManagement.Core.BiqQuery;

namespace CurrencyManagement.Services.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBigQuery _bigQuery;

        public CurrencyService(IUnitOfWork unitOfWork, IBigQuery bigQuery)
        {
            _unitOfWork = unitOfWork;
            _bigQuery = bigQuery;
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

            await _bigQuery.InsertCurrencyDimension(entity.CurrencyDimensions.First());
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

            if (currency == null)
            {
                throw new KeyNotFoundException("Currency not found");
            }

            if (currency.CurrentPriceInUSD <= -increasePrice)
            {
                throw new CurrencyPriceLessThanZeroException(currency.Name);
            }

            currency.CurrentPriceInUSD += increasePrice;

            return await UpdateAsync(currency);
        }

        public async Task FillingData(int id)
        {
            Random random = new Random();

            for (int i = 0; i < 10000; i++)
            {
                await UpdatePriceAsync(
                    id,
                    (decimal)random.Next(-100, 100) / 100
                    );
            }
        }

        private async Task _AddDimension(Currency entity)
        {
            var updatedEntity = await _unitOfWork.CurrencyRepository
                .GetByIdWithDimensionAsync(entity.Id);

            if (updatedEntity == null)
            {
                throw new KeyNotFoundException("Currency not found");
            }

            var lastDim = updatedEntity.CurrencyDimensions.First(c => c.IsCurrent == true);
            lastDim.EndDate = DateTime.Now;
            lastDim.IsCurrent = false;

            updatedEntity.CurrencyDimensions.Add(
                new CurrencyDimension()
                {
                    FromDate = DateTime.Now,
                    PriceInUSD = entity.CurrentPriceInUSD
                });
        }
    }
}

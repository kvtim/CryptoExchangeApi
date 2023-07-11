using CurrencyManagement.Core.Cassandra;
using CurrencyManagement.Core.Models;
using CurrencyManagement.Core.Repositories;
using CurrencyManagement.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyManagement.Services.Services
{
    public class CassandraService : ICassandraService
    {
        ICassandraRepository _cassandraRepository;
        IRepository<CurrencyDimension> _repository;

        public CassandraService(
            ICassandraRepository cassandraRepository,
            IRepository<CurrencyDimension> repository)
        { 
            _cassandraRepository = cassandraRepository;
            _repository = repository;
        }

        public async Task InsertAll()
        {
            var currencyDimensions = await _repository.GetAllAsync();
            await _cassandraRepository.InsertList(currencyDimensions.ToList());
        }

        public async Task<List<CurrencyDimension>> GetAll()
        {
            var dimensions = await _cassandraRepository.GetAll();

            if (!dimensions.Any())
            {
                throw new KeyNotFoundException("Dimensions not found");
            }

            return dimensions;
        }

        public async Task<CurrencyDimension> GetById(int id)
        {
            var dimension = await _cassandraRepository.GetById(id);

            if (dimension == null)
            {
                throw new KeyNotFoundException("Dimension not found");
            }

            return dimension;
        }

        public async Task<List<CurrencyDimension>> GetDimensionsBetweenDates(
            DateTime fromDate,
            DateTime endDate)
        {
            var dimensions = await _cassandraRepository.GetDimensionsBetweenDates(
                fromDate, 
                endDate);

            if (!dimensions.Any())
            {
                throw new KeyNotFoundException("Dimensions not found");
            }

            return dimensions;
        }
    }
}

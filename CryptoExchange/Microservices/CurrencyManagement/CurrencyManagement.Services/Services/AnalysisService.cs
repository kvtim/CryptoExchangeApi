using CurrencyManagement.Core.BiqQuery;
using CurrencyManagement.Core.Dtos.Analys;
using CurrencyManagement.Core.Models;
using CurrencyManagement.Core.Repositories;
using CurrencyManagement.Core.Services;
using Google.Cloud.BigQuery.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyManagement.Services.Services
{
    public class AnalysisService : IAnalysisService
    {
        IRepository<CurrencyDimension> _repository;
        IBigQuery _bigQuery;

        public AnalysisService(
            IRepository<CurrencyDimension> repository,
            IBigQuery bigQuery)
        {
            _repository = repository;
            _bigQuery = bigQuery;
        }

        public async Task InsertDataToBigQuery()
        {
            var currencyDimensions = await _repository.GetAllAsync();
                
            await _bigQuery.InsertCurrencyDimensionList(currencyDimensions.ToList());
        }

        public async Task<List<CurrencyDimension>> GetAllDimensions()
        {
            return await _bigQuery.GetAllRows();
        }

        public async Task<DifferentBetweenDatesDto> GetDifferentBetweenFirstAndLastDate(int currencyId)
        {
            var (different, percents) = await _bigQuery.
                GetDifferentBetweenFirstAndLastDate(currencyId);

            return new DifferentBetweenDatesDto()
            {
                Different = different,
                Percents = percents,
                Message = $"Currency {currencyId} changed by {different} $ or {percents} %"
            };
        }

        public async Task<DifferentBetweenDifferentDatesDto> GetDifferentBetweenDifferentDates(
            int currencyId,
            DateTime fromDate,
            DateTime endDate)
        {
            var (different, percents) = await _bigQuery.
                GetDifferentBetweenDifferentDates(currencyId, fromDate, endDate);

            return new DifferentBetweenDifferentDatesDto()
            {
                Different = different,
                Percents = percents,
                FromDate = fromDate,
                EndDate = endDate,
                Message = $"Currency {currencyId} from {fromDate} to {endDate}" +
                $" changed by {different} $ or {percents} %"
            };
        }
    }
}

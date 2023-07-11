using Cassandra;
using Cassandra.Mapping;
using CurrencyManagement.Core.Cassandra;
using CurrencyManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyManagement.Data.Cassandra
{
    public class CassandraRepository : ICassandraRepository
    {
        private readonly IMapper _mapper;
        public CassandraRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task InsertList(List<CurrencyDimension> currencyDimensions)
        {
            var batch = _mapper.CreateBatch();

            currencyDimensions.ForEach(d => batch.Insert(d));

            var consistency = ConsistencyLevel.One;
            batch.WithOptions(o => o.SetConsistencyLevel(consistency));

            await _mapper.ExecuteAsync(batch);
        }

        public async Task<List<CurrencyDimension>> GetAll()
        {
            var currencyDimensions = await _mapper.FetchAsync<CurrencyDimension>();

            return currencyDimensions.ToList();
        }

        public async Task<CurrencyDimension> GetById(int id)
        {
            var currencyDimensions = await _mapper
                .FetchAsync<CurrencyDimension>("WHERE dimension_id = ?", id);

            return currencyDimensions.FirstOrDefault();
        }

        public async Task<List<CurrencyDimension>> GetDimensionsBetweenDates(
            DateTime fromDate,
            DateTime endDate)
        {
            var currencyDimensions = await _mapper
                .FetchAsync<CurrencyDimension>(
                $"WHERE fromdate >= '{fromDate.ToString("yyyy-MM-dd HH:mm:ss")}' " +
                $"AND enddate <= '{endDate.ToString("yyyy-MM-dd HH:mm:ss")}' " +
                $"ALLOW FILTERING");

            return currencyDimensions.ToList();
        }
    }
}

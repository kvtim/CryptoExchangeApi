using CurrencyManagement.Core.BiqQuery;
using CurrencyManagement.Core.Models;
using Google.Apis.Bigquery.v2.Data;
using Google.Cloud.BigQuery.V2;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyManagement.Data.BigQuery
{
    public class BigQuery : IBigQuery
    {
        private const string PROJECT_ID = "currencyproject-384111";

        private async Task<BigQueryClient> _GetBigqueryClient()
        {
            BigQueryClient client = await BigQueryClient.CreateAsync(PROJECT_ID);
            return client;
        }

        public async Task<List<CurrencyDimension>> GetAllRows()
        {
            var client = await _GetBigqueryClient();

            string tableName = "`currencyproject-384111.currency_dataset.currency_dimension`";

            var query = $"SELECT * FROM {tableName};";

            BigQueryParameter[] parameters = null;
            BigQueryResults results = await client.ExecuteQueryAsync(query, parameters);

            var currencyDimensions = _BigQueryRowToCuurencyDimension(results.ToList());
            return currencyDimensions;
        }

        public async Task<(double, double)> GetDifferentBetweenFirstAndLastDate(int currencyId)
        {
            var client = await _GetBigqueryClient();

            BigQueryParameter[] parameters = new[]
{
                new BigQueryParameter(
                    "CurrencyIdParam",
                    BigQueryDbType.Int64,
                    currencyId)
            };

            string tableName = "`currencyproject-384111.currency_dataset.currency_dimension`";

            string query = $"SELECT PriceInUSD FROM {tableName} " +
                $"WHERE CurrencyId = @CurrencyIdParam " +
                $"ORDER BY FromDate;";

            BigQueryResults priceResult = await client.ExecuteQueryAsync(
                query,
                parameters);

            return _CalculateDifferent(priceResult);
        }

        public async Task<(double, double)> GetDifferentBetweenDifferentDates(
            int currencyId,
            DateTime fromDate,
            DateTime endDate)
        {
            var client = await _GetBigqueryClient();

            BigQueryParameter[] parameters = new[]
{
                new BigQueryParameter(
                    "CurrencyIdParam",
                    BigQueryDbType.Int64,
                    currencyId),

                new BigQueryParameter(
                    "FromDateParam",
                    BigQueryDbType.Timestamp,
                    fromDate.ToUniversalTime()),

                new BigQueryParameter(
                    "EndDateParam",
                    BigQueryDbType.Timestamp,
                    endDate.ToUniversalTime())

            };

            string tableName = "`currencyproject-384111.currency_dataset.currency_dimension`";

            string query = $"SELECT PriceInUSD FROM {tableName} " +
                $"WHERE CurrencyId = @CurrencyIdParam " +
                $"AND FromDate >= @FromDateParam " +
                $"AND EndDate <= @EndDateParam " +
                $"ORDER BY FromDate;";

            BigQueryResults priceResult = await client.ExecuteQueryAsync(
                query,
                parameters);

            return _CalculateDifferent(priceResult);
        }

        public async Task InsertCurrencyDimensionList(List<CurrencyDimension> currencyDimensions)
        {
            BigQueryTable table = await _GetTable();

            List<BigQueryInsertRow> rows = new List<BigQueryInsertRow>();

            List<(int Id, int CurrencyId)> currencyDimensionsIds = await _GetLastDimensionsIds();

            if (currencyDimensionsIds.Any())
            {
                foreach (var currencyDimensionId in currencyDimensionsIds)
                {
                    if (!currencyDimensions
                        .First(d => d.Id == currencyDimensionId.Id)
                        .IsCurrent)
                    {
                        await UpdateDimension(
                        currencyDimensionId.Id,
                        currencyDimensions
                        .First(d => d.Id == currencyDimensionId.Id)
                        .EndDate.Value
                        );

                        rows.AddRange(
                            _CuurencyDimensionToBigQueryInsertRow(
                                currencyDimensions.Where(d =>
                                d.Id > currencyDimensionId.Id &&
                                d.CurrencyId == currencyDimensionId.CurrencyId
                                ).ToList()));
                    }
                }

            }
            else
            {
                rows = _CuurencyDimensionToBigQueryInsertRow(currencyDimensions);
            }

            if (!rows.Any())
            {
                throw new ArgumentException("Nothing to upload");
            }

            await table.InsertRowsAsync(rows);

        }

        public async Task InsertCurrencyDimension(CurrencyDimension dimension)
        {
            BigQueryTable table = await _GetTable();

            await table.InsertRowAsync(new BigQueryInsertRow
            {
                { "Id", dimension.Id },
                { "PriceInUSD", (float)dimension.PriceInUSD },
                { "FromDate", dimension.FromDate.GetValueOrDefault().ToUniversalTime() },
                { "EndDate", dimension.EndDate.GetValueOrDefault().ToUniversalTime() },
                { "IsCurrent", dimension.IsCurrent },
                { "CurrencyId", dimension.CurrencyId }
            });
        }

        public async Task UpdateDimension(int id, DateTime endDate)
        {
            var client = await _GetBigqueryClient();

            BigQueryParameter[] parameters = new[]
            {
                new BigQueryParameter(
                    "IdParam",
                    BigQueryDbType.Int64,
                    id),

                new BigQueryParameter(
                    "EndDateParam",
                    BigQueryDbType.Timestamp,
                    endDate.ToUniversalTime()),

                new BigQueryParameter(
                    "IsCurrentParam",
                    BigQueryDbType.Bool,
                    false)
            };

            string tableName = "`currencyproject-384111.currency_dataset.currency_dimension`";

            string query = $"UPDATE {tableName} " +
                $"SET EndDate = @EndDateParam " +
                $", IsCurrent = @IsCurrentParam " +
                $"WHERE Id = @IdParam;";

            await client.ExecuteQueryAsync(
                query,
                parameters);
        }

        private (double, double) _CalculateDifferent(BigQueryResults priceResult)
        {
            double firstPrice = (double)priceResult.First()["PriceInUSD"];
            double secondPrice = (double)priceResult.Last()["PriceInUSD"];

            var different = secondPrice - firstPrice;
            var percents = different / firstPrice * 100;
            return (different, percents);
        }

        private async Task<BigQueryTable> _GetTable()
        {
            BigQueryClient client = await _GetBigqueryClient();

            BigQueryDataset dataset = await client.GetOrCreateDatasetAsync("currency_dataset");

            return await dataset.GetOrCreateTableAsync(
                "currency_dimension", new TableSchemaBuilder
                {
                    { "Id", BigQueryDbType.Int64 },
                    { "PriceInUSD", BigQueryDbType.Float64 },
                    { "FromDate", BigQueryDbType.Timestamp },
                    { "EndDate", BigQueryDbType.Timestamp },
                    { "IsCurrent", BigQueryDbType.Bool },
                    { "CurrencyId", BigQueryDbType.Int64 }
                }.Build());
        }

        private List<CurrencyDimension> _BigQueryRowToCuurencyDimension(
            List<BigQueryRow> bigQueryRow)
        {
            List<CurrencyDimension> currencyDimensions = new List<CurrencyDimension>();

            foreach (var row in bigQueryRow)
            {
                currencyDimensions.Add(new CurrencyDimension
                {
                    Id = (int)(long)row["Id"],
                    PriceInUSD = (decimal)(double)row["PriceInUSD"],
                    FromDate = (DateTime)row["FromDate"],
                    EndDate = (DateTime?)row["EndDate"],
                    IsCurrent = (bool)row["IsCurrent"],
                    CurrencyId = (int)(long)row["CurrencyId"]
                });
            }

            return currencyDimensions;
        }

        private List<BigQueryInsertRow> _CuurencyDimensionToBigQueryInsertRow(
            List<CurrencyDimension> currencyDimensions)
        {
            List<BigQueryInsertRow> rows = new List<BigQueryInsertRow>();

            foreach (CurrencyDimension dimension in currencyDimensions)
            {
                rows.Add(new BigQueryInsertRow
                {
                    { "Id", dimension.Id },
                    { "PriceInUSD", (float)dimension.PriceInUSD },
                    { "FromDate", dimension.FromDate.GetValueOrDefault().ToUniversalTime() },
                    { "EndDate", dimension.EndDate.GetValueOrDefault().ToUniversalTime() },
                    { "IsCurrent", dimension.IsCurrent },
                    { "CurrencyId", dimension.CurrencyId }
                });
            }

            return rows;
        }


        public async Task<List<(int Id, int CurrencyId)>> _GetLastDimensionsIds()
        {
            var client = await _GetBigqueryClient();

            BigQueryParameter[] parameters = new[]
{
                new BigQueryParameter(
                    "IsCurrentParam",
                    BigQueryDbType.Bool,
                    true)
            };

            string tableName = "`currencyproject-384111.currency_dataset.currency_dimension`";

            string query = $"SELECT Id, CurrencyId FROM {tableName} " +
                $"WHERE IsCurrent = @IsCurrentParam;";

            BigQueryResults result = await client.ExecuteQueryAsync(
                query,
                parameters);

            List<(int, int)> currentDimIds = new List<(int, int)>();

            foreach (var row in result.ToList())
            {
                currentDimIds.Add(
                    ((int)(long)row["Id"],
                    (int)(long)row["CurrencyId"])
                    );
            }

            return currentDimIds;
        }
    }
}

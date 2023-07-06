using CurrencyManagement.Core.Models;
using Cassandra.Mapping;

namespace CurrencyManagement.Data.CassandraMappings
{
    public class CurrencyDimensionMapping : Mappings
    {
        public CurrencyDimensionMapping()
        {
            For<CurrencyDimension>().TableName("currency_dimension")
               .PartitionKey(u => u.Id)
               .Column(x => x.Id, cm => cm.WithName("dimension_id"))
               .Column(x => x.PriceInUSD)
               .Column(x => x.FromDate)
               .Column(x => x.EndDate)
               .Column(x => x.IsCurrent)
               .Column(x => x.CurrencyId)
               .ExplicitColumns();
        }
    }
}

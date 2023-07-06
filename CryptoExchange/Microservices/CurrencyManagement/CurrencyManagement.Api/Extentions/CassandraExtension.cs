
using Cassandra;
using Cassandra.Mapping;
using CurrencyManagement.Data.CassandraMappings;
using ISession = Cassandra.ISession;

namespace CurrencyManagement.Api.Extentions
{
    public static class CassandraExtension
    {
        public static void ConfigureCassandra(this IServiceCollection services)
        {
            var cluster = Cluster.Builder()
                     .AddContactPoints("cassandra")
                     .Build();

            ISession session = cluster.Connect();

            session.Execute("CREATE KEYSPACE IF NOT EXISTS currencies " +
                "WITH REPLICATION = { 'class' : 'SimpleStrategy', 'replication_factor' : 3 };");

            session = cluster.Connect("currencies");

            string createTableUserQuery = "CREATE TABLE IF NOT EXISTS currency_dimension("
                + "dimension_id int PRIMARY KEY, "
                + "priceinusd decimal, "
                + "fromdate timestamp, "
                + "enddate timestamp, "
                + "iscurrent boolean, "
                + "currencyid int);";

            session.Execute(createTableUserQuery);

            MappingConfiguration.Global.Define<CurrencyDimensionMapping>();

            IMapper mapper = new Mapper(session);

            services.AddSingleton<IMapper>(mapper);
        }
    }
}

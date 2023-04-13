using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CurrencyManagement.Core.Models;

namespace CurrencyManagement.Data.Seed
{
    public class CurrencySeed : IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            builder.HasData(
                new Currency
                {
                    Id = 1,
                    Name = "USD",
                    CurrentPriceInUSD = 1.0m,
                    CurrencyType = CurrencyType.Money,
                });

            builder.HasData(
                new Currency
                {
                    Id = 2,
                    Name = "EUR",
                    CurrentPriceInUSD = 1.11m,
                    CurrencyType = CurrencyType.Money,
                });

            builder.HasData(
                new Currency
                {
                    Id = 3,
                    Name = "RUB",
                    CurrentPriceInUSD = 0.012m,
                    CurrencyType = CurrencyType.Money,
                });

            builder.HasData(
                new Currency
                {
                    Id = 4,
                    Name = "BTC",
                    CurrentPriceInUSD = 30288.8m,
                    CurrencyType = CurrencyType.Crypto,
                });

            builder.HasData(
                new Currency
                {
                    Id = 5,
                    Name = "ETH",
                    CurrentPriceInUSD = 2005.87m,
                    CurrencyType = CurrencyType.Crypto,
                });

            builder.HasData(
                new Currency
                {
                    Id = 6,
                    Name = "LTC",
                    CurrentPriceInUSD = 94.13m,
                    CurrencyType = CurrencyType.Crypto,
                });
        }
    }
}

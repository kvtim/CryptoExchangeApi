using CurrencyManagement.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyManagement.Data.Seed
{
    public class CurrencyDimensionSeed : IEntityTypeConfiguration<CurrencyDimension>
    {
        public void Configure(EntityTypeBuilder<CurrencyDimension> builder)
        {
            builder.HasData(
                new CurrencyDimension()
                {
                    Id = 1,
                    CurrencyId = 1,
                    PriceInUSD = 1.0m,
                });

            builder.HasData(
                new CurrencyDimension()
                {
                    Id = 2,
                    CurrencyId = 2,
                    PriceInUSD = 1.11m,
                });

            builder.HasData(
                new CurrencyDimension()
                {
                    Id = 3,
                    CurrencyId = 3,
                    PriceInUSD = 0.012m
                });

            builder.HasData(
                new CurrencyDimension()
                {
                    Id = 4,
                    CurrencyId = 4,
                    PriceInUSD = 30288.8m
                });

            builder.HasData(
                new CurrencyDimension()
                {
                    Id = 5,
                    CurrencyId = 5,
                    PriceInUSD = 2005.87m
                });

            builder.HasData(
                new CurrencyDimension()
                {
                    Id = 6,
                    CurrencyId = 6,
                    PriceInUSD = 94.13m
                });
        }
    }
}

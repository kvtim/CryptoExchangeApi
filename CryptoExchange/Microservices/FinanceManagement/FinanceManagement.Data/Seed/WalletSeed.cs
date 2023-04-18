using FinanceManagement.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Data.Seed
{
    public class WalletSeed : IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> builder)
        {
            builder.HasData(
                new Wallet()
                {
                    Id = 1,
                    CurrencyId = 1,
                    UserId = 1,
                    CurrencyAmount = 100000.0m
                });

            builder.HasData(
                new Wallet()
                {
                    Id = 2,
                    CurrencyId = 1,
                    UserId = 2,
                    CurrencyAmount = 0.0m
                });
        }
    }
}

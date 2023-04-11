using FinanceManagement.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Data.Configuration
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.Property(t => t.FromCurrencyId)
                .IsRequired();

            builder.Property(t => t.FromCurrencyPricePerUnit)
                .IsRequired();

            builder.Property(t => t.NewCurrencyId)
                .IsRequired();

            builder.Property(t => t.NewCurrencyAmount)
                .IsRequired();
            
            builder.Property(t => t.NewCurrencyPricePerUnit)
                .IsRequired();

            builder.Property(t => t.UserId)
                .IsRequired();
            
            builder.Property(t => t.FullTransactionPriceUSD)
                .IsRequired();

            builder.Property(t => t.TransactionDate)
                .HasDefaultValue(DateTime.Now)
                .IsRequired();
        }
    }
}

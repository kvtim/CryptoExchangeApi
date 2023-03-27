using CurrencyManagement.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyManagement.Data.Configuration
{
    public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            builder.Property(c => c.Name)
                .HasMaxLength(128)
                .IsRequired();

            builder.Property(c => c.CurrentPriceInUSD)
                .IsRequired();

            builder
                .HasIndex(u => u.Name)
                .IsUnique();
        }
    }
}

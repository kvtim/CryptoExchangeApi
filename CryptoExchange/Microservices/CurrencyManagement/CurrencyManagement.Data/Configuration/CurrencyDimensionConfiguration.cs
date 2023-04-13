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
    public class CurrencyDimensionConfiguration : IEntityTypeConfiguration<CurrencyDimension>
    {
        public void Configure(EntityTypeBuilder<CurrencyDimension> builder)
        {
            builder.HasOne(c => c.Currency)
                .WithMany(cd => cd.CurrencyDimensions)
                .HasForeignKey(c => c.CurrencyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(c => c.PriceInUSD)
                .IsRequired();

            builder.Property(c => c.FromDate)
                .IsRequired();

            builder.Property(c => c.IsCurrent)
                .HasDefaultValue(true);
        }
    }
}

﻿// <auto-generated />
using System;
using FinanceManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FinanceManagement.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("FinanceManagement.Core.Models.Transaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("FromCurrencyId")
                        .HasColumnType("integer");

                    b.Property<decimal>("FromCurrencyPricePerUnit")
                        .HasColumnType("numeric");

                    b.Property<decimal>("FullTransactionPriceUSD")
                        .HasColumnType("numeric");

                    b.Property<decimal>("NewCurrencyAmount")
                        .HasColumnType("numeric");

                    b.Property<int>("NewCurrencyId")
                        .HasColumnType("integer");

                    b.Property<decimal>("NewCurrencyPricePerUnit")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("TransactionDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasDefaultValue(new DateTime(2023, 4, 4, 17, 35, 20, 544, DateTimeKind.Local).AddTicks(545));

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("FinanceManagement.Core.Models.Wallet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<decimal>("CurrencyAmount")
                        .HasColumnType("numeric");

                    b.Property<int>("CurrencyId")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Wallets");
                });
#pragma warning restore 612, 618
        }
    }
}

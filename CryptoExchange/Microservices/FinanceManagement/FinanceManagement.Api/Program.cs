using FinanceManagement.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using FinanceManagement.Data.Wallets.Commands.CreateWallet;
using FinanceManagement.Core.Repositories;
using FinanceManagement.Data.Repositories;
using FinanceManagement.Api.Extensions;
using MassTransit;
using FinanceManagement.Api.EventBusConsumer;
using EventBus.Messages.Common;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddJWTAuthentication(builder.Configuration);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IWalletRepository, WalletRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

builder.Services.AddMediatrWithConfiguration();

builder.Services.AddControllersWithJsonConfiguration();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<CreateNewUserWalletConsumer>();
    config.UsingRabbitMq((ctx, cfg) => 
    {
        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
        cfg.ReceiveEndpoint(EventBusConstants.CreateNewUserWallet, c => {
            c.ConfigureConsumer<CreateNewUserWalletConsumer>(ctx);
        });
    });
});

builder.Services.AddScoped<CreateNewUserWalletConsumer>();

builder.Services.ConfigureSwagger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

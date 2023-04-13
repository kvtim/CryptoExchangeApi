using CurrencyManagement.Api.Middlewares;
using CurrencyManagement.Api.Extensions;
using CurrencyManagement.Data;
using Microsoft.EntityFrameworkCore;
using CurrencyManagement.Api.Extentions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

app.MigrateIfDbNotCreated();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

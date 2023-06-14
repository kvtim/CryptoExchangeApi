using Elasticsearch.Net;
using FinanceManagement.Api.Extensions;
using FinanceManagement.Data;
using FinanceManagement.Data.Elasticsearch;
using FinanceManagement.Data.Logger;
using Microsoft.EntityFrameworkCore;
using Nest;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    if (!scope.ServiceProvider.
        GetRequiredService<ApplicationDbContext>().
        Database.CanConnect())
    {
        scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();
    }
}
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

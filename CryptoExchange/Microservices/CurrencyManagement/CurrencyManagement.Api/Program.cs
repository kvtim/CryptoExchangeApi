using CurrencyManagement.Api.Middlewares;
using CurrencyManagement.Api.Extensions;
using CurrencyManagement.Data;
using Microsoft.EntityFrameworkCore;

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

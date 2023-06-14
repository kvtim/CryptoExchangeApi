using Microsoft.EntityFrameworkCore;
using UserManagement.Api.Extensions;
using UserManagement.Data;

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

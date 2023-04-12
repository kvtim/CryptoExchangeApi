using CurrencyManagement.Core.Repositories;
using CurrencyManagement.Core.UnitOfWork;
using CurrencyManagement.Data.Repositories;
using CurrencyManagement.Data.UnitOfWork;
using CurrencyManagement.Data;
using CurrencyManagement.Core.Services;
using CurrencyManagement.Services.Services;
using Microsoft.EntityFrameworkCore;
using CurrencyManagement.Api.Middlewares;
using CurrencyManagement.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddJWTAuthentication(builder.Configuration);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<ICurrencyService, CurrencyService>();

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.ConfigureSwagger();

var app = builder.Build();


// Configure the HTTP request pipeline.
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

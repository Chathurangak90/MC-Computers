using AutoMapper;
using MCComputers.Repositories.DBContext;
using MCComputers.Repositories.Interfaces;
using MCComputers.Repositories.Mapper;
using MCComputers.Repositories.Repository;
using MCComputers.Services.Interfaces;
using MCComputers.Services.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Cors
builder.Services.AddCors(x => x.AddPolicy("corspolicy", build =>
{
    build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));


var connectionString = builder.Configuration.GetConnectionString("MCDBContext");

if (string.IsNullOrEmpty(connectionString))
{
    throw new Exception("Database connection string is missing from appsettings.json");
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>(), AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IInvoiceService, InvoiceService>();;
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();;

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("corspolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

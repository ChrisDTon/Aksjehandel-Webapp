using aksjehandel.DAL;
using aksjehandel.DAL.StockLayer;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var databaseConnection = builder.Configuration.GetConnectionString("DatabaseConnection");

builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<IPurchaseRepository, PurchaseRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

// Add Databases
builder.Services.AddDbContext<Context>(options => options.UseSqlite(databaseConnection));

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(3600);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add services to the container.
var logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .Enrich.FromLogContext()
  .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

var app = builder.Build();

app.UseSession();

if (app.Environment.IsDevelopment())
{

    app.UseDeveloperExceptionPage();
    DBInit.Initialize(app);
}

app.UseCors(options =>
{
    options.AllowAnyOrigin();
    options.AllowAnyMethod();
});

app.UseStatusCodePagesWithReExecute("/");
app.UseDefaultFiles();
app.UseStaticFiles();
app.MapControllers();

app.Run();

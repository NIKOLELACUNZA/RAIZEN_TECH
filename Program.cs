using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PRUEBA.Data;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

Console.WriteLine("111");

var connectionString = builder.Configuration.GetConnectionString("PostgreSQLConnection") ?? 
throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

Console.WriteLine(connectionString);
Console.WriteLine("1222");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
   options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddStackExchangeRedisCache(options =>
    {
        var configuration = builder.Configuration.GetSection("Caching:RedisCache");
        options.Configuration = configuration["ConnectionString"];
    });

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PRUEBA.Models;

namespace PRUEBA.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<PRODUCT>? PRODUCTs{get; set;}
    public DbSet<USERS>? USERs{get; set;}
    public DbSet<ORDER>? ORDERs{get; set;}
    public DbSet<ORDER_ITEM>? ORDER_ITEMs{get;set;}
    public DbSet<CATEGORY>? CATEGORYs{get; set;}
    public DbSet<CART>? CARTs{get; set;}
    public DbSet<CART_ITEM>? CART_ITEMs{get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<CART>()
        .HasOne(c => c.USERs)
        .WithOne(u => u.CARTs)
        .HasForeignKey<USERS>(u => u.Id_User);

    modelBuilder.Entity<CART_ITEM>()
        .HasOne(c => c.ORDER_ITEMs)
        .WithOne(u => u.CART_ITEMs)
        .HasForeignKey<ORDER_ITEM>(u => u.Id_OrderItem);

}



}

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
    public DbSet<PAY>? PAYs{get; set;}
    
}


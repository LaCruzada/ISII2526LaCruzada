using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AppForSEII2526.API.Models;

namespace AppForSEII2526.API.Data;




public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options) {

    // Entidades existentes
    public DbSet<TipoProducto> TipoProducto { get; set; }
    public DbSet<Compra_Producto> Compra_Producto { get; set; }
    public DbSet<Compra> Compra { get; set; }
    public DbSet<CompraBocadillo> CompraBocadillo { get; set; }
    public DbSet<ProductoCompra> ProductoCompra { get; set; }
    public DbSet<Producto> Producto { get; set; }
    public DbSet<TipoBocadillo> TipoBocadillos { get; set; }
    public DbSet<BonoBocadillo> BonoBocadillo { get; set; }
    public DbSet<BonosComprados> BonosComprados { get; set; }
    public DbSet<CompraBono> CompraBono { get; set; }
    public DbSet<Bocadillo> Bocadillos { get; set; }
    public DbSet<TipoPan> TipoPanes { get; set; }
    public DbSet<Resenya> Resenyas { get; set; }
    public DbSet<ResenyaBocadillo> ResenyaBocadillo { get; set; }
    public DbSet<ResenyaBocadillo> BocadilloId { get; set; }
    public DbSet<ApplicationUser> usuarios { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ProductoCompra>().HasKey(pc => new { pc.CompraId, pc.ProductoId });
        builder.Entity<BonosComprados>().HasKey(pi => new { pi.BonoId, pi.CompraBonoId });
        builder.Entity<ResenyaBocadillo>().HasKey(pc => new { pc.BocadilloId, pc.ResenyaId });
        builder.Entity<CompraBocadillo>().HasKey(pc => new { pc.CompraId, pc.BocadilloId });
        
  
        builder.Entity<TipoProducto>().HasData(
            new TipoProducto { TipoProductoId = 1, Nombre = "Camiseta" },
            new TipoProducto { TipoProductoId = 2, Nombre = "Taza" },
            new TipoProducto { TipoProductoId = 3, Nombre = "Gorro" }
        );


        builder.Entity<Producto>().HasData(
            new Producto { ProductoId = 1, Nombre = "Camiseta Logo", PVP = 15.00m, Stock = 10, TipoProductoId = 1 },
            new Producto { ProductoId = 2, Nombre = "Camiseta Vintage", PVP = 18.50m, Stock = 3, TipoProductoId = 1 },
            new Producto { ProductoId = 3, Nombre = "Taza Café", PVP = 8.99m, Stock = 20, TipoProductoId = 2 },
            new Producto { ProductoId = 4, Nombre = "Gorro Invierno", PVP = 12.50m, Stock = 5, TipoProductoId = 3 }
        );

    }
}

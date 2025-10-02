using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AppForSEII2526.API.Models;

namespace AppForSEII2526.API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options) {

    // Entidades existentes
    public DbSet<TipoBocadillo> TipoBocadillos { get; set; }
    public DbSet<BonoBocadillo> BonoBocadillo { get; set; }
    public DbSet<BonosComprados> BonosComprados { get; set; }
    public DbSet<CompraBono> CompraBono { get; set; }

    // Nuevas entidades para productos y compras
    public DbSet<TipoProducto> TipoProductos { get; set; }
    public DbSet<Producto> Productos { get; set; }
    public DbSet<ProductoCompra> ProductoCompras { get; set; }
    public DbSet<Compra> Compras { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configuración existente
        builder.Entity<BonosComprados>().HasKey(pi => new { pi.compraBonoId, pi.bonoId });

        // Configuración para las nuevas entidades
        
        // ProductoCompra tiene clave compuesta (CompraId, ProductoId)
        builder.Entity<ProductoCompra>()
            .HasKey(pc => new { pc.CompraId, pc.ProductoId });

        // Relación TipoProducto -> Producto (1:N)
        builder.Entity<Producto>()
            .HasOne(p => p.TipoProducto)
            .WithMany(tp => tp.Productos)
            .HasForeignKey(p => p.TipoProductoId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relación Producto -> ProductoCompra (1:N)
        builder.Entity<ProductoCompra>()
            .HasOne(pc => pc.Producto)
            .WithMany(p => p.ProductoCompras)
            .HasForeignKey(pc => pc.ProductoId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relación Compra -> ProductoCompra (1:N)
        builder.Entity<ProductoCompra>()
            .HasOne(pc => pc.Compra)
            .WithMany(c => c.ProductoCompras)
            .HasForeignKey(pc => pc.CompraId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configuraciones adicionales de precisión decimal
        builder.Entity<Producto>()
            .Property(p => p.PVP)
            .HasPrecision(18, 2);

        builder.Entity<ProductoCompra>()
            .Property(pc => pc.PVP)
            .HasPrecision(18, 2);

        builder.Entity<Compra>()
            .Property(c => c.PrecioFinal)
            .HasPrecision(18, 2);
    }
}

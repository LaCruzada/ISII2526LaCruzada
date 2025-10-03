using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AppForSEII2526.API.Models;

namespace AppForSEII2526.API.Data;




public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options) {

    // Entidades existentes
    public DbSet<TipoProducto> TipoProducto { get; set; }
    public DbSet<Compra> Compra { get; set; }
    public DbSet<ProductoCompra> ProductoCompra { get; set; }
    public DbSet<Producto> Producto { get; set; }
    public DbSet<TipoBocadillo> TipoBocadillos { get; set; }
    public DbSet<BonoBocadillo> BonoBocadillo { get; set; }
    public DbSet<BonosComprados> BonosComprados { get; set; }
    public DbSet<CompraBono> CompraBono { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configuración de clave compuesta para ProductoCompra
        builder.Entity<ProductoCompra>().HasKey(pc => new { pc.CompraId, pc.ProductoId });
    }
        builder.Entity<BonosComprados>().HasKey(pi => new { pi.compraBonoId, pi.bonoId });
    }


}

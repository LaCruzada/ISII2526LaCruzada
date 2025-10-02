using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppForSEII2526.API.Models
{
    public class Producto
    {
        public Producto()
        {
            ProductoCompras = new List<ProductoCompra>();
        }

        public Producto(int productoId, string nombre, decimal pvp, int stock, int tipoProductoId)
        {
            ProductoId = productoId;
            Nombre = nombre;
            PVP = pvp;
            Stock = stock;
            TipoProductoId = tipoProductoId;
            ProductoCompras = new List<ProductoCompra>();
        }

        [Key]
        public int ProductoId { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PVP { get; set; }

        [Required]
        public int Stock { get; set; }

        // Foreign Key hacia TipoProducto
        [Required]
        public int TipoProductoId { get; set; }

        // Relación: Un Producto pertenece a un TipoProducto
        [ForeignKey("TipoProductoId")]
        public TipoProducto TipoProducto { get; set; }

        // Relación: Un Producto puede aparecer en muchos ProductoCompra
        public ICollection<ProductoCompra> ProductoCompras { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Producto producto &&
                   ProductoId == producto.ProductoId &&
                   Nombre == producto.Nombre &&
                   PVP == producto.PVP &&
                   Stock == producto.Stock &&
                   TipoProductoId == producto.TipoProductoId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ProductoId, Nombre, PVP, Stock, TipoProductoId);
        }
    }
}

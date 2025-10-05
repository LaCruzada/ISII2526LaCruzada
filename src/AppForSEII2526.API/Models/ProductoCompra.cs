using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppForSEII2526.API.Models
{
    public class ProductoCompra
    {
        public ProductoCompra()
        {
        }

        public ProductoCompra(int compraId, int productoId, int cantidad, decimal pvp)
        {
            CompraId = compraId;
            ProductoId = productoId;
            Cantidad = cantidad;
            PVP = pvp;
        }

        // Clave compuesta: CompraId + ProductoId
        [Required]
        public int CompraId { get; set; }

        [Required]
        public int ProductoId { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PVP { get; set; }

        // Relación: Muchos ProductoCompra pertenecen a una Compra
        [ForeignKey("CompraId")]
        public Compra_Producto Compra { get; set; }

        // Relación: Un ProductoCompra pertenece a un Producto
        [ForeignKey("ProductoId")]
        public Producto Producto { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ProductoCompra productoCompra &&
                   CompraId == productoCompra.CompraId &&
                   ProductoId == productoCompra.ProductoId &&
                   Cantidad == productoCompra.Cantidad &&
                   PVP == productoCompra.PVP;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CompraId, ProductoId, Cantidad, PVP);
        }
    }
}
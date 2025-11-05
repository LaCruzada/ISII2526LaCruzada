using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using DataType = System.ComponentModel.DataAnnotations.DataType;

namespace AppForSEII2526.API.Models
{
    public class Compra_Producto
    {
        public Compra_Producto(int compraId, List<ApplicationUser> usuario, string direccionEnvio, DateTime fechaCompra, string metodo_Pago, decimal precioFinal, List<ProductoCompra> productoCompras)
        {
            CompraId = compraId;
            this.usuario = usuario;
            DireccionEnvio = direccionEnvio;
            FechaCompra = fechaCompra;
            Metodo_Pago = metodo_Pago;
            PrecioFinal = precioFinal;
            ProductoCompras = productoCompras;
        }

        public Compra_Producto()
        {
        }

        [Key]
        public int CompraId { get; set; }

        public List<ApplicationUser> usuario { get; set; } = new List<ApplicationUser>();
        [Required]
        [StringLength(200)]
        public string DireccionEnvio { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayName("Fecha de Compra")]
        public DateTime FechaCompra { get; set; }

        [Required]
        [StringLength(50)]
        public string Metodo_Pago { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioFinal { get; set; }

        // Relación: Una Compra tiene muchos ProductoCompra
        public List<ProductoCompra> ProductoCompras { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Compra_Producto producto &&
                   CompraId == producto.CompraId &&
                   EqualityComparer<List<ApplicationUser>>.Default.Equals(usuario, producto.usuario) &&
                   DireccionEnvio == producto.DireccionEnvio &&
                   FechaCompra == producto.FechaCompra &&
                   Metodo_Pago == producto.Metodo_Pago &&
                   PrecioFinal == producto.PrecioFinal &&
                   EqualityComparer<List<ProductoCompra>>.Default.Equals(ProductoCompras, producto.ProductoCompras);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CompraId, usuario, DireccionEnvio, FechaCompra, Metodo_Pago, PrecioFinal, ProductoCompras);
        }
    }
}
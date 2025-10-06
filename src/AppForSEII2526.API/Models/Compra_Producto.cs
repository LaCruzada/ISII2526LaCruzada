using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using DataType = System.ComponentModel.DataAnnotations.DataType;

namespace AppForSEII2526.API.Models
{
    public class Compra_Producto
    {
        public Compra_Producto()
        {

            ProductoCompras = new List<ProductoCompra>();
            Nombre = string.Empty;
            Apellido_1 = string.Empty;
            Apellido_2 = string.Empty;
            DireccionEnvio = string.Empty;
            Metodo_Pago = string.Empty;
        }

        public Compra_Producto(int compraId, string nombre, string apellido1, string apellido2,
                     string direccionEnvio, DateTime fechaCompra, string metodoPago, decimal precioFinal)
        {
            CompraId = compraId;
            Nombre = nombre;
            Apellido_1 = apellido1;
            Apellido_2 = apellido2;
            DireccionEnvio = direccionEnvio;
            FechaCompra = fechaCompra;
            Metodo_Pago = metodoPago;
            PrecioFinal = precioFinal;
            ProductoCompras = new List<ProductoCompra>();
        }

        [Key]
        public int CompraId { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(50)]
        public string Apellido_1 { get; set; }

        [StringLength(50)]
        public string Apellido_2 { get; set; }

        [Required]
        [StringLength(200)]
        public string DireccionEnvio { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayName("Fecha de Compra")]
        public DateTime FechaCompra { get; set; }

        [Required]
        [StringLength(50)]
        public string Metodo_Pago { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioFinal { get; set; }

        // Relación: Una Compra tiene muchos ProductoCompra
        public List<ProductoCompra> ProductoCompras { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Compra_Producto compra &&
                   CompraId == compra.CompraId &&
                   Nombre == compra.Nombre &&
                   Apellido_1 == compra.Apellido_1 &&
                   Apellido_2 == compra.Apellido_2 &&
                   DireccionEnvio == compra.DireccionEnvio &&
                   FechaCompra == compra.FechaCompra &&
                   Metodo_Pago == compra.Metodo_Pago &&
                   PrecioFinal == compra.PrecioFinal;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CompraId, Nombre, Apellido_1, Apellido_2, DireccionEnvio, FechaCompra, Metodo_Pago, PrecioFinal);
        }
    }
}
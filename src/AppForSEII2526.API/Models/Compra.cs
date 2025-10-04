using System.ComponentModel;
using DataType = System.ComponentModel.DataAnnotations.DataType;

namespace AppForSEII2526.API.Models
{
    public class Compra
    {
        public Compra()
        {
            ProductoCompras = new List<ProductoCompra>();
            Nombre = string.Empty;
            Apellido_1 = string.Empty;
            Apellido_2 = string.Empty;
            DireccionEnvio = string.Empty;
            Metodo_Pago = string.Empty;
        }
       
        public Compra(int compraId, string nombre, string apellido1, string apellido2, 
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

        [DataType(DataType.Date), Display(Name = "Fecha Bono")]
        public DateTime FechaCompra { get; set; }

        [StringLength(50)]
        public string Apellido_2 { get; set; }

        [Required]

        [Required]


        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioFinal { get; set; }
        

        public override bool Equals(object? obj)
        {
            return obj is Compra compra &&
                   CompraId == compra.CompraId &&
                   Nombre == compra.Nombre &&
                   Apellido_1 == compra.Apellido_1 &&
                   Apellido_2 == compra.Apellido_2 &&
                   DireccionEnvio == compra.DireccionEnvio &&
                   FechaCompra == compra.FechaCompra &&
        }

        public override int GetHashCode()
        {
        }
    }
}
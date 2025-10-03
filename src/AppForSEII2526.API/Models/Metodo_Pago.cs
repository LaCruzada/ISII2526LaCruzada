using System.ComponentModel.DataAnnotations;

namespace AppForSEII2526.API.Models
{
    public enum Metodo_Pago// hacer clase abtracta erencua
    {
        Tarjeta,
        PayPal,
        GPay
    }

    public class Compra
    {
        [Key]
        public int CompraId { get; set; }

        [Required]
        public string NombreCliente { get; set; }

        [Required]
        public string Apellido1Cliente { get; set; }

        public string Apellido2Cliente { get; set; } 

        [Required]
        public DateTime FechaCompra { get; set; }

        public int nBocadillos => BocadillosComprados?.Count ?? 0;

        [Column(TypeName = "decimal(10,2)")]
        public decimal PrecioTotal { get; set; }

        [Required]
        public Metodo_Pago MetodoPago { get; set; }

        public ICollection<CompraBocadillo> BocadillosComprados { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace AppForSEII2526.API.Models
{
    public enum Metodo_Pago
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
        [StringLength(100)]
        public string NombreCliente { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Apellido1 { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Apellido2 { get; set; }

        public DateTime FechaCompra { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioTotal { get; set; }

        public Metodo_Pago MetodoPago { get; set; }

        public ICollection<CompraBocadillo> BocadillosComprados { get; set; } = new List<CompraBocadillo>();
    }
}

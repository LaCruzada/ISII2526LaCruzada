using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppForSEII2526.API.Models
{
    public class CompraBocadillo
    {
        [Key]
        public int CompraBocadilloId { get; set; }

        public int BocadilloId { get; set; }

        public int CompraId { get; set; }

        public int Cantidad { get; set; }

        [Required]
        [StringLength(200)]
        public string NombreBocadillo { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Precio { get; set; }

        public int TipoPanId { get; set; }

        [ForeignKey("BocadilloId")]
        public Bocadillo? Bocadillo { get; set; }

        [ForeignKey("CompraId")]
        public Compra? Compra { get; set; }

        [ForeignKey("TipoPanId")]
        public TipoPan? TipoPan { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppForSEII2526.API.Models
{
    public class Bocadillo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Nombre { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal PVP { get; set; }

        public int Stock { get; set; }

        [StringLength(500)]
        public string ResenyaBocadillo { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Tamano { get; set; } = string.Empty;

        public int TipoPanId { get; set; }

        [ForeignKey("TipoPanId")]
        public TipoPan? TipoPan { get; set; }

        public ICollection<CompraBocadillo> ComprasDelBocadillo { get; set; } = new List<CompraBocadillo>();
    }
}

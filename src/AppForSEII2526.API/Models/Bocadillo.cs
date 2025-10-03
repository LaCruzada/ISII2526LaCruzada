using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppForSEII2526.API.Models
{
    public class Bocadillo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal PVP { get; set; }

        public string ResenyaBocadillo { get; set; }

        public int Stock { get; set; }

        public string Tamano { get; set; }

        public int PanId { get; set; }
        public TipoPan TipoPan { get; set; }

        public ICollection<CompraBocadillo> ComprasDelBocadillo { get; set; }
    }
}

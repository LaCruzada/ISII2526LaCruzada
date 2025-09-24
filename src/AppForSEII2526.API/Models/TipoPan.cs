using System.ComponentModel.DataAnnotations;

namespace AppForSEII2526.API.Models
{
    public class TipoPan
    {
        [Key]
        public int TipoPanId { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        public ICollection<Bocadillo> Bocadillos { get; set; } = new List<Bocadillo>();
    }
}

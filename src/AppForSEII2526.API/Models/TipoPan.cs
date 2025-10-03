using System.ComponentModel.DataAnnotations;

namespace AppForSEII2526.API.Models
{
    public class TipoPan
    {
        [Key]
        public int PanId { get; set; }

        [Required]
        public string Nombre { get; set; }

        public ICollection<Bocadillo> Bocadillos { get; set; }
    }
}

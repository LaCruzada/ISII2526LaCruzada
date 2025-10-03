using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppForSEII2526.API.Models
{
    public class CompraBocadillo
    {
        [Key]
        public int Id { get; set; }

        public int BocadilloId { get; set; }
        public Bocadillo Bocadillo { get; set; }

        public int CompraId { get; set; }
        public Compra Compra { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Precio
        {
            get; set;
        }
    }
}
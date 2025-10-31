using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppForSEII2526.API.Models
{
    public class CompraBocadillo
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string NombreBocadillo { get; set; }

        public TipoPan Tipopan { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Precio { get; set; }
        public CompraBocadillo()
        {
        }

        public CompraBocadillo(int id, string nombreBocadillo, TipoPan tipopan, int cantidad, decimal precio)
        {
            Id = id;
            NombreBocadillo = nombreBocadillo;
            Tipopan = tipopan;
            Cantidad = cantidad;
            Precio = precio;
        }

        public override bool Equals(object? obj)
        {
            return obj is CompraBocadillo bocadillo &&
                   Id == bocadillo.Id &&
                   NombreBocadillo == bocadillo.NombreBocadillo &&
                   EqualityComparer<TipoPan>.Default.Equals(Tipopan, bocadillo.Tipopan) &&
                   Cantidad == bocadillo.Cantidad &&
                   Precio == bocadillo.Precio;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, NombreBocadillo, Tipopan, Cantidad, Precio);
        }
    }
}
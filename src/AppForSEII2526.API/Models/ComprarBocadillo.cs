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
        public override bool Equals(object? obj)
        {
            return obj is CompraBocadillo bocadillo &&
                   Id == bocadillo.Id &&
                   BocadilloId == bocadillo.BocadilloId &&
                   EqualityComparer<Bocadillo>.Default.Equals(Bocadillo, bocadillo.Bocadillo) &&
                   CompraId == bocadillo.CompraId &&
                   EqualityComparer<Compra>.Default.Equals(Compra, bocadillo.Compra) &&
                   Cantidad == bocadillo.Cantidad &&
                   Precio == bocadillo.Precio;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Id, BocadilloId, Bocadillo, CompraId, Compra, Cantidad, Precio);
        }
        public CompraBocadillo()
        {
        }
        public CompraBocadillo(int id, int bocadilloId, Bocadillo bocadillo, int compraId, Compra compra, int cantidad, decimal precio)
        {
            Id = id;
            BocadilloId = bocadilloId;
            Bocadillo = bocadillo;
            CompraId = compraId;
            Compra = compra;
            Cantidad = cantidad;
            Precio = precio;
        }
    }
}
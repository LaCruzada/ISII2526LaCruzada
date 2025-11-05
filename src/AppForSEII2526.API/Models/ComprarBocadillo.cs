using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppForSEII2526.API.Models
{
    public class CompraBocadillo
    {
       
        public int CompraId { get; set; }
        public int BocadilloId { get; set; }

        [Required]
        public int Cantidad { get; set; }

        public Compra Compra { get; set; }
        public Bocadillo Bocadillo { get; set; }


        public CompraBocadillo()
        {
        }

        public CompraBocadillo(int compraId, int bocadilloId, int cantidad)
        {
            CompraId = compraId;
            BocadilloId = bocadilloId;
            Cantidad = cantidad;
        }

        public override bool Equals(object? obj)
        {
            return obj is CompraBocadillo bocadillo &&
                   CompraId == bocadillo.CompraId &&
                   BocadilloId == bocadillo.BocadilloId &&
                   Cantidad == bocadillo.Cantidad &&
                   EqualityComparer<Compra>.Default.Equals(Compra, bocadillo.Compra) &&
                   EqualityComparer<Bocadillo>.Default.Equals(Bocadillo, bocadillo.Bocadillo);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CompraId, BocadilloId, Cantidad, Compra, Bocadillo);
        }
    }
}
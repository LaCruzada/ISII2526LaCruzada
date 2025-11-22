
namespace AppForSEII2526.API.DTOs.DTOsPedirBocadillo
{
    public class BocadilloItemDTO
    {
        public int BocadilloId { get; set; }
        public string NombreBocadillo { get; set; }
        public string TipoPan { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is BocadilloItemDTO dTO &&
                   BocadilloId == dTO.BocadilloId &&
                   NombreBocadillo == dTO.NombreBocadillo &&
                   TipoPan == dTO.TipoPan &&
                   Cantidad == dTO.Cantidad &&
                   PrecioUnitario == dTO.PrecioUnitario;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(BocadilloId, NombreBocadillo, TipoPan, Cantidad, PrecioUnitario);
        }
    }
}
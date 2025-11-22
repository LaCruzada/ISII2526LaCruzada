
namespace AppForSEII2526.API.DTOs.DTOsPedirBocadillo
{
    public class BocadilloPedidoItemDTO
    {
        [Required]
        public int BocadilloId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1")]
        public int Cantidad { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is BocadilloPedidoItemDTO dTO &&
                   BocadilloId == dTO.BocadilloId &&
                   Cantidad == dTO.Cantidad;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(BocadilloId, Cantidad);
        }
    }
}

namespace AppForSEII2526.API.DTOs.DTOsPedirBocadillo
{
    public class PedidoBocadilloDetailsDTO
    {
        public int PedidoId { get; set; }
        public string NombreCliente { get; set; } 
        public DateTime FechaCompra { get; set; }
        public string MetodoPago { get; set; }
        public decimal PrecioTotal { get; set; }
        public List<BocadilloItemDTO> Bocadillos { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is PedidoBocadilloDetailsDTO dTO &&
                   PedidoId == dTO.PedidoId &&
                   NombreCliente == dTO.NombreCliente &&
                   FechaCompra == dTO.FechaCompra &&
                   MetodoPago == dTO.MetodoPago &&
                   PrecioTotal == dTO.PrecioTotal &&
                   EqualityComparer<List<BocadilloItemDTO>>.Default.Equals(Bocadillos, dTO.Bocadillos);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PedidoId, NombreCliente, FechaCompra, MetodoPago, PrecioTotal, Bocadillos);
        }
    }
}
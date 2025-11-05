namespace AppForSEII2526.API.DTOs
{
    public class PedidoBocadilloDetailsDTO
    {
        public int PedidoId { get; set; }
        public string NombreCliente { get; set; } 
        public DateTime FechaCompra { get; set; }
        public string MetodoPago { get; set; }
        public decimal PrecioTotal { get; set; }
        public List<BocadilloItemDTO> Bocadillos { get; set; }
    }
}
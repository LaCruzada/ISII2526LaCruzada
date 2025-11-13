// File: DTOs/DTOsCompraMerchandising/CompraMerchandisingDetailsDTO.cs
namespace AppForSEII2526.API.DTOs.DTOsCompraMerchandising
{
    public class CompraMerchandisingDetailsDTO
    {
        public int CompraId { get; set; }
        public string NombreCliente { get; set; } = string.Empty;
        public string DireccionEnvio { get; set; } = string.Empty;
        public string MetodoPago { get; set; } = string.Empty;
        public DateTime FechaCompra { get; set; }
        public decimal PrecioTotal { get; set; }
        public List<ProductoItemDTO> Productos { get; set; } = new();
    }

    public class ProductoItemDTO
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public decimal PrecioUnitario { get; set; }
        public int Cantidad { get; set; }
    }
}
// File: DTOs/DTOsCompraMerchandising/ComprarMerchandisingCreateDTO.cs
namespace AppForSEII2526.API.DTOs.DTOsCompraMerchandising
{
    public class ComprarMerchandisingCreateDTO
    {
        public string EmailCliente { get; set; } = string.Empty;
        public string NombreCliente { get; set; } = string.Empty;
        public string Apellido1Cliente { get; set; } = string.Empty;
        public string? Apellido2Cliente { get; set; }
        public string DireccionEnvio { get; set; } = string.Empty;
        public string MetodoPago { get; set; } = string.Empty;
        public List<ProductoCantidadDTO> Productos { get; set; } = new();
    }

    public class ProductoCantidadDTO
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
    }
}
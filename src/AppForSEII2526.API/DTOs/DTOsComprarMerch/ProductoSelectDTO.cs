// File: DTOs/DTOsCompraMerchandising/ProductoSelectDTO.cs
namespace AppForSEII2526.API.DTOs.DTOsCompraMerchandising
{
    public class ProductoSelectDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public int Stock { get; set; }
    }
}
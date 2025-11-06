namespace AppForSEII2526.API.DTOs.DTOsPedirBocadillo
{
    public class BocadilloItemDTO
    {
        public int BocadilloId { get; set; }
        public string NombreBocadillo { get; set; }
        public string TipoPan { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; } 
    }
}
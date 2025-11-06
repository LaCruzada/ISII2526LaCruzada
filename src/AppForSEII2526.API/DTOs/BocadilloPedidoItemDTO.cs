namespace AppForSEII2526.API.DTOs
{
    public class BocadilloPedidoItemDTO
    {
        [Required]
        public int BocadilloId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1")]
        public int Cantidad { get; set; }
    }
}

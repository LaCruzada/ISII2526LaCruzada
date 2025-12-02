namespace AppForSEII2526.API.ComprarBonoBocadilloDTOs
{
    // DTO auxiliar para visualizar bonos en el carrito (crea este archivo o ajusta según tu estructura)
    public class BonoSelectDTO
    {
        public int BonoId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public double PVP { get; set; }
        public int NBocadillos { get; set; }
        public string TipoBocadillo { get; set; } = string.Empty;
        public int CantidadDisponible { get; set; }
    }
}

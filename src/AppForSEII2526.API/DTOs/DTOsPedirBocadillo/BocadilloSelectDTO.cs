namespace AppForSEII2526.API.DTOs.DTOsPedirBocadillo
{
    //Para el BocadilloSelectController
    public class BocadilloSelectDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; } 
        public string TipoPan { get; set; }
        public string Tamano { get; set; } 
        public int Stock { get; set; }
    }
}

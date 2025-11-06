namespace AppForSEII2526.API.ComprarBonoBocadilloDTOs
{
    public class ComprarBonosDTO
    {
        public ComprarBonosDTO()
        {
        }

        public ComprarBonosDTO(int bonoID, string nombre, double precioCompra, int cantidad, string tipo)
        {
            BonoID = bonoID;
            Nombre = nombre;
            PrecioCompra = precioCompra;
            Cantidad = cantidad;
            Tipo = tipo;
        }

        [JsonPropertyName("BonoID")]
        public int BonoID { get; set; }

        [StringLength(20, ErrorMessage = "El nombre del bocadillo no puede ser mayor de 20 Caracteres .")]
        [JsonPropertyName("Nombre")]
        public string Nombre { get; set; }

        [Display(Name = "Precio de la Compra")]
        [JsonPropertyName("PrecioDeLaCompra")]
        public double PrecioCompra { get; set; }

        [Required]
        [JsonPropertyName("Cantidad")]
        [Range(1, Double.MaxValue, ErrorMessage = "Debes de indicar una cantidad válida.")]
        public int Cantidad { get; set; }

        [JsonPropertyName("Tipo")]
        public string Tipo { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ComprarBonosDTO dTO &&
                   BonoID == dTO.BonoID &&
                   Nombre == dTO.Nombre &&
                   PrecioCompra == dTO.PrecioCompra &&
                   Cantidad == dTO.Cantidad &&
                   Tipo == dTO.Tipo;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(BonoID, Nombre, PrecioCompra, Cantidad, Tipo);
        }
    }

}

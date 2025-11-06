namespace AppForSEII2526.API.ComprarBonoBocadilloDTOs
{
    public class BonosCompradosDTO
    {
        public BonosCompradosDTO()
        {
        }

        public BonosCompradosDTO(int bonoID, string nombre, double precioUnitario, int cantidad, string tipo)
        {
            BonoID = bonoID;
            Nombre = nombre;
            PrecioUnitario = precioUnitario;
            Cantidad = cantidad;
            Tipo = tipo;
        }

        [JsonPropertyName("BonoID")]
        public int BonoID { get; set; }

        [StringLength(20, ErrorMessage = "El nombre del portátil no puede ser más de 20 caracteres.")]
        [JsonPropertyName("nombre")]
        public string Nombre { get; set; }

        [Display(Name = "Preciode la Compra")]
        [JsonPropertyName("precioUnitario")]
        [Required]
        public double PrecioUnitario { get; set; }

        [Required]
        [JsonPropertyName("Cantidad")]
        [Range(1, Double.MaxValue, ErrorMessage = "Debes de indicar una cantidad válida.")]
        public int Cantidad { get; set; }

        [JsonPropertyName("Tipo")]
        public string Tipo { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is BonosCompradosDTO dTO &&
                   BonoID == dTO.BonoID &&
                   Nombre == dTO.Nombre &&
                   PrecioUnitario == dTO.PrecioUnitario &&
                   Cantidad == dTO.Cantidad &&
                   Tipo == dTO.Tipo;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(BonoID, Nombre, PrecioUnitario, Cantidad, Tipo);
        }
    }
}

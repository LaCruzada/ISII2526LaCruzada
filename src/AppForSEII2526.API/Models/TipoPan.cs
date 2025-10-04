using System.ComponentModel.DataAnnotations;


namespace AppForSEII2526.API.Models
{
    public class TipoPan
    {
        [Key]
        public int PanId { get; set; }

        [Required, StringLength(20, ErrorMessage = "El nombre no puede ocupar más de 20 caracteres.", MinimumLength = 1)]
        public string? Nombre { get; set; }

        public List<Bocadillo> Bocadillos { get; set; }

        public TipoPan()
        {
            Bocadillos = new List<Bocadillo>();
        }
        public TipoPan(int panId, string nombre, List<Bocadillo> bocadillos)
        {
            PanId = panId;
            Nombre = nombre;
            Bocadillos = bocadillos;
        }

        public override bool Equals(object? obj)
        {
            return obj is TipoPan pan &&
                   PanId == pan.PanId &&
                   Nombre == pan.Nombre &&
                   EqualityComparer<List<Bocadillo>>.Default.Equals(Bocadillos, pan.Bocadillos);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PanId, Nombre, Bocadillos);
        }
    }
}

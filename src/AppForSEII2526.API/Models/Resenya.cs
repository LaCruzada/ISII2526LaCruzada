using DataType = System.ComponentModel.DataAnnotations.DataType;

namespace AppForSEII2526.API.Models
{
    public class Resenya
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(200, ErrorMessage = "La descripción no puede tener más de 200 caracteres.", MinimumLength = 1)]
        public string? descripcion { get; set; }

        [DataType(DataType.DateTime), Display(Name = "Fecha Reseña")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaInicio { get; set; }

        [StringLength(20, ErrorMessage = "El nombre no puede ocupar más de 20 caracteres.", MinimumLength = 1)]
        public string NombreUsuario { get; set; }

        [StringLength(20, ErrorMessage = "El titulo no puede ocupar más de 20 caracteres.", MinimumLength = 1)]
        public string Titulo { get; set; }

        public EnumValoracion_General Valoracion_General { get; set; }

        public List<ResenyaBocadillo> ResenyaBocadillos { get; set; } = new List<ResenyaBocadillo>();

        public Resenya()
        {
            ResenyaBocadillos = new List<ResenyaBocadillo>();
        }
        public Resenya(int id, string descripcion, DateTime fechaInicio, string nombreUsuario, string titulo, EnumValoracion_General valoracion_General, List<ResenyaBocadillo> resenyaBocadillo)
        {
            Id = id;
            this.descripcion = descripcion;
            FechaInicio = fechaInicio;
            NombreUsuario = nombreUsuario;
            Titulo = titulo;
            Valoracion_General = valoracion_General;
            ResenyaBocadillos = resenyaBocadillo;
        }

        public override bool Equals(object? obj)
        {
            return obj is Resenya resenya &&
                   Id == resenya.Id &&
                   descripcion == resenya.descripcion &&
                   FechaInicio == resenya.FechaInicio &&
                   NombreUsuario == resenya.NombreUsuario &&
                   Titulo == resenya.Titulo &&
                   Valoracion_General == resenya.Valoracion_General &&
                   EqualityComparer<List<ResenyaBocadillo>>.Default.Equals(ResenyaBocadillos, resenya.ResenyaBocadillos);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, descripcion, FechaInicio, NombreUsuario, Titulo, Valoracion_General, ResenyaBocadillos);
        }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace AppForSEII2526.API.Models
{
    public class Bocadillo
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(20, ErrorMessage = "El nombre no puede ocupar más de 20 caracteres.", MinimumLength = 1)]
        public string Nombre { get; set; }

        [Required, Column(TypeName = "decimal(10,2)")]
        public decimal PVP { get; set; }

        [Required]
        public EnumTamaño Tamano { get; set; }

        [Required]
        public int TipoPanId { get; set; }
        public List<CompraBocadillo> CompraBocadillos { get; set; } = new List<CompraBocadillo>();


        public int Stock { get; set; }

        public List<ResenyaBocadillo> ResenyaBocadillos { get; set; } = new List<ResenyaBocadillo>();

        public TipoPan TipoPan { get; set; }
        public Bocadillo()
        {

        }

        public Bocadillo(int id, string nombre, decimal pVP, EnumTamaño tamano, int tipoPanId, List<CompraBocadillo> compraBocadillos, int stock, List<ResenyaBocadillo> resenyaBocadillos, TipoPan tipoPan)
        {
            Id = id;
            Nombre = nombre;
            PVP = pVP;
            Tamano = tamano;
            TipoPanId = tipoPanId;
            CompraBocadillos = compraBocadillos;
            Stock = stock;
            ResenyaBocadillos = resenyaBocadillos;
            TipoPan = tipoPan;
        }

        public Bocadillo(int id, string nombre, decimal pVP, EnumTamaño tamano, int tipoPanId, List<ResenyaBocadillo> resenyaBocadillos)
        {
            Id = id;
            Nombre = nombre;
            PVP = pVP;
            Tamano = tamano;
            TipoPanId = tipoPanId;
            ResenyaBocadillos = resenyaBocadillos;
        }

        public override bool Equals(object? obj)
        {
            return obj is Bocadillo bocadillo &&
                   Id == bocadillo.Id &&
                   Nombre == bocadillo.Nombre &&
                   PVP == bocadillo.PVP &&
                   Tamano == bocadillo.Tamano &&
                   TipoPanId == bocadillo.TipoPanId &&
                   EqualityComparer<List<CompraBocadillo>>.Default.Equals(CompraBocadillos, bocadillo.CompraBocadillos) &&
                   Stock == bocadillo.Stock &&
                   EqualityComparer<List<ResenyaBocadillo>>.Default.Equals(ResenyaBocadillos, bocadillo.ResenyaBocadillos) &&
                   EqualityComparer<TipoPan>.Default.Equals(TipoPan, bocadillo.TipoPan);
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Id);
            hash.Add(Nombre);
            hash.Add(PVP);
            hash.Add(Tamano);
            hash.Add(TipoPanId);
            hash.Add(CompraBocadillos);
            hash.Add(Stock);
            hash.Add(ResenyaBocadillos);
            hash.Add(TipoPan);
            return hash.ToHashCode();
        }
    }
}
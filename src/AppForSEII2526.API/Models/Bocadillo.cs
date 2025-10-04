using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace AppForSEII2526.API.Models
{
    public class Bocadillo
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(20, ErrorMessage = "El nombre no puede ocupar más de 20 caracteres.", MinimumLength = 1)]
        public string? Nombre { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal PVP { get; set; }
        public List<CompraBocadillo> CompraBocadillos { get; set; } = new List<CompraBocadillo>();

        public string ResenyaBocadillo { get; set; }

        public int Stock { get; set; }

        public int TipoPanId { get; set; }

        public EnumTamaño Tamano { get; set; }

        public List<ResenyaBocadillo> ResenyaBocadillos { get; set; } = new List<ResenyaBocadillo>();

        public TipoPan TipoPan { get; set; }
        public Bocadillo()
        {
            ResenyaBocadillos = new List<ResenyaBocadillo>();
        }

        public Bocadillo(int id, string? nombre, decimal pVP, List<CompraBocadillo> comprasDelBocadillo, string resenyaBocadillo, int stock, int panId, EnumTamaño tamano, List<ResenyaBocadillo> resenyaBocadillos, TipoPan tipoPan)
        {
            Id = id;
            Nombre = nombre;
            PVP = pVP;
            CompraBocadillos = comprasDelBocadillo;
            ResenyaBocadillo = resenyaBocadillo;
            Stock = stock;
            TipoPanId = panId;
            Tamano = tamano;
            ResenyaBocadillos = resenyaBocadillos;
            TipoPan = tipoPan;
        }

        public override bool Equals(object? obj)
        {
            return obj is Bocadillo bocadillo &&
                   Id == bocadillo.Id &&
                   Nombre == bocadillo.Nombre &&
                   PVP == bocadillo.PVP &&
                   EqualityComparer<List<CompraBocadillo>>.Default.Equals(CompraBocadillos, bocadillo.CompraBocadillos) &&
                   ResenyaBocadillo == bocadillo.ResenyaBocadillo &&
                   Stock == bocadillo.Stock &&
                   TipoPanId == bocadillo.TipoPanId &&
                   Tamano == bocadillo.Tamano &&
                   EqualityComparer<List<ResenyaBocadillo>>.Default.Equals(ResenyaBocadillos, bocadillo.ResenyaBocadillos) &&
                   EqualityComparer<TipoPan>.Default.Equals(TipoPan, bocadillo.TipoPan);
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Id);
            hash.Add(Nombre);
            hash.Add(PVP);
            hash.Add(CompraBocadillos);
            hash.Add(ResenyaBocadillo);
            hash.Add(Stock);
            hash.Add(TipoPanId);
            hash.Add(Tamano);
            hash.Add(ResenyaBocadillos);
            hash.Add(TipoPan);
            return hash.ToHashCode();
        }
    }
}

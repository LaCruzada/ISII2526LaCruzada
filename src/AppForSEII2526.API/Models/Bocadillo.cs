

namespace AppForSEII2526.API.Models
{
    public class Bocadillo
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(20, ErrorMessage = "El nombre no puede ocupar más de 20 caracteres.", MinimumLength = 1)]
        public string? Nombre { get; set; }

        public int ComprasDelBocadillo { get; set; }

        public int stock { get; set; }

        [Column(TypeName = "float(10,2)")]
        public float PVP { get; set; }

        public int TipoPanId { get; set; }

        public EnumTamaño Tamano { get; set; }

        public List<ResenyaBocadillo> ResenyaBocadillos { get; set; } = new List<ResenyaBocadillo>();

        public TipoPan? TipoPan { get; set; }

        public Bocadillo()
        {
            ResenyaBocadillos = new List<ResenyaBocadillo>();
        }
        public Bocadillo(int id, string nombre, int comprasDelBocadillo, int stock, float pVP, int tipoPanId, EnumTamaño tamano, List<ResenyaBocadillo> resenyaBocadillo, TipoPan? tipoPan)
        {
            Id = id;
            Nombre = nombre;
            ComprasDelBocadillo = comprasDelBocadillo;
            this.stock = stock;
            PVP = pVP;
            TipoPanId = tipoPanId;
            Tamano = tamano;
            ResenyaBocadillos = resenyaBocadillo;
            TipoPan = tipoPan;
        }

        public override bool Equals(object? obj)
        {
            return obj is Bocadillo bocadillo &&
                   Id == bocadillo.Id &&
                   Nombre == bocadillo.Nombre &&
                   ComprasDelBocadillo == bocadillo.ComprasDelBocadillo &&
                   stock == bocadillo.stock &&
                   PVP == bocadillo.PVP &&
                   TipoPanId == bocadillo.TipoPanId &&
                   Tamano == bocadillo.Tamano &&
                   EqualityComparer<List<ResenyaBocadillo>>.Default.Equals(ResenyaBocadillos, bocadillo.ResenyaBocadillos) &&
                   EqualityComparer<TipoPan?>.Default.Equals(TipoPan, bocadillo.TipoPan);
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Id);
            hash.Add(Nombre);
            hash.Add(ComprasDelBocadillo);
            hash.Add(stock);
            hash.Add(PVP);
            hash.Add(TipoPanId);
            hash.Add(Tamano);
            hash.Add(ResenyaBocadillos);
            hash.Add(TipoPan);
            return hash.ToHashCode();
        }
    }
}

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
        public int ComprasDelBocadillo { get; set; }

        public string ResenyaBocadillo { get; set; }

        public int Stock { get; set; }
        [Column(TypeName = "float(10,2)")]
        public float PVP { get; set; }

        public string Tamano { get; set; }

        public int PanId { get; set; }

        public EnumTamaño Tamano { get; set; }

        public List<ResenyaBocadillo> ResenyaBocadillos { get; set; } = new List<ResenyaBocadillo>();

        public Bocadillo()
        {
            ResenyaBocadillos = new List<ResenyaBocadillo>();
        }
        public Bocadillo(int id, string nombre, int comprasDelBocadillo, int stock, float pVP, int panId, EnumTamaño tamano, List<ResenyaBocadillo> resenyaBocadillo, TipoPan? tipoPan)
        {
            Id = id;
            Nombre = nombre;
            ComprasDelBocadillo = comprasDelBocadillo;
            this.stock = stock;
            PVP = pVP;
            PanId = panId;
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
        public TipoPan TipoPan { get; set; }

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
            hash.Add(PanId);
            hash.Add(Tamano);
            hash.Add(ResenyaBocadillos);
            hash.Add(TipoPan);
            return hash.ToHashCode();
        public ICollection<CompraBocadillo> ComprasDelBocadillo { get; set; }
    }
}

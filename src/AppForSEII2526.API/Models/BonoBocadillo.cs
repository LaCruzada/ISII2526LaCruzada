
namespace AppForSEII2526.API.Models
{
    public class BonoBocadillo
    {
        public BonoBocadillo()
        {
        }

        public BonoBocadillo(int bonoId, int cantidadDisponible, int nBocadillos, string nombre, float pVP, TipoBocadillo tipoBocadillos, List<BonosComprados> bonosComprados)
        {
            BonoId = bonoId;
            this.cantidadDisponible = cantidadDisponible;
            this.nBocadillos = nBocadillos;
            this.nombre = nombre;
            PVP = pVP;
            this.tipoBocadillos = tipoBocadillos;
            this.bonosComprados = bonosComprados;
        }

        [Key]
        public int BonoId {  get; set; }
        [Required]
        public int cantidadDisponible {  get; set; }
        [Required]
        public int nBocadillos {  get; set; }
        [Required]
        public string nombre { get; set;}
        [Required]
        public float PVP { get; set; } 

        public TipoBocadillo tipoBocadillos { get; set; }
        public List<BonosComprados> bonosComprados { get; set; } = new List<BonosComprados>();

        public override bool Equals(object? obj)
        {
            return obj is BonoBocadillo bocadillo &&
                   BonoId == bocadillo.BonoId &&
                   cantidadDisponible == bocadillo.cantidadDisponible &&
                   nBocadillos == bocadillo.nBocadillos &&
                   nombre == bocadillo.nombre &&
                   PVP == bocadillo.PVP &&
                   EqualityComparer<TipoBocadillo>.Default.Equals(tipoBocadillos, bocadillo.tipoBocadillos) &&
                   EqualityComparer<List<BonosComprados>>.Default.Equals(bonosComprados, bocadillo.bonosComprados);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(BonoId, cantidadDisponible, nBocadillos, nombre, PVP, tipoBocadillos, bonosComprados);
        }
    }
}



namespace AppForSEII2526.API.Models
{
    public class BonosComprados
    {
        public BonosComprados()
        {
        }

        public BonosComprados(int bonoId, int cantidad, int compraId, float precioBono, CompraBono compra, BonoBocadillo bono)
        {
            this.BonoId = bonoId;
            Cantidad = cantidad;
            CompraId = compraId;
            PrecioBono = precioBono;
            Compra = compra;
            Bono = bono;
         
        }

        [Key]
        public int BonoId {  get; set; }
        [Required]
        public int Cantidad {  get; set; }

        [Required]
        public int CompraId { get; set; }

        [Required]
        public float PrecioBono { get; set; }

        public CompraBono Compra { get; set; } 

        public BonoBocadillo Bono { get; set; }


        public override bool Equals(object? obj)
        {
            return obj is BonosComprados comprados &&
                   BonoId == comprados.BonoId &&
                   Cantidad == comprados.Cantidad &&
                   CompraId == comprados.CompraId &&
                   PrecioBono == comprados.PrecioBono &&
                   EqualityComparer<CompraBono>.Default.Equals(Compra, comprados.Compra) &&
                   EqualityComparer<BonoBocadillo>.Default.Equals(Bono, comprados.Bono);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(BonoId, Cantidad, CompraId, PrecioBono, Compra, Bono);
        }
    }
}

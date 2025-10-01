

namespace AppForSEII2526.API.Models
{
    public class BonosComprados
    {
        public BonosComprados()
        {
        }

        public BonosComprados(int bonoId, int cantidad, int compraId, float precioBono, CompraBono compra, int compraBonoId, BonoBocadillo bono, int bonosId)
        {
            this.BonosId = bonoId;
            Cantidad = cantidad;
            CompraId = compraId;
            PrecioBono = precioBono;
            Compra = compra;
            this.compraBonoId = compraBonoId;
            Bono = bono;
            this.bonoId = bonosId;
        }

        [Key]
        public int BonosId {  get; set; }
        [Required]
        public int Cantidad {  get; set; }

        [Required]
        public int CompraId { get; set; }

        [Required]
        public float PrecioBono { get; set; }

        public CompraBono Compra { get; set; } 

        public int compraBonoId { get; set; }
        
        public BonoBocadillo Bono { get; set; }

        public int bonoId { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is BonosComprados comprados &&
                   BonosId == comprados.BonosId &&
                   Cantidad == comprados.Cantidad &&
                   CompraId == comprados.CompraId &&
                   PrecioBono == comprados.PrecioBono &&
                   EqualityComparer<CompraBono>.Default.Equals(Compra, comprados.Compra) &&
                   compraBonoId == comprados.compraBonoId &&
                   EqualityComparer<BonoBocadillo>.Default.Equals(Bono, comprados.Bono) &&
                   bonoId == comprados.bonoId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(BonosId, Cantidad, CompraId, PrecioBono, Compra, compraBonoId, Bono, bonoId);
        }
    }
}

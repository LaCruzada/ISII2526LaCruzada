namespace AppForSEII2526.API.DTOs.DTOsCrearResenya.BocadillosDTOs
{
    public class BocadilloItemResenya
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public decimal PVP { get; set; }

        public EnumTamaño Tamano { get; set; }

        public int TipoPanId { get; set; }

        public BocadilloItemResenya() { }

        public BocadilloItemResenya(int id, string nombre, decimal pvp, EnumTamaño tamano, int tipoPanId)
        {
            Id = id;
            Nombre = nombre ?? throw new ArgumentNullException(nameof(nombre));
            PVP = pvp;
            Tamano = tamano;
            TipoPanId = tipoPanId;
        }

        public override bool Equals(object? obj)
        {
            return obj is BocadilloItemResenya resenya &&
                   Id == resenya.Id &&
                   Nombre == resenya.Nombre &&
                   PVP == resenya.PVP &&
                   Tamano == resenya.Tamano &&
                   TipoPanId == resenya.TipoPanId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Nombre, PVP, Tamano, TipoPanId);
        }
    }
}

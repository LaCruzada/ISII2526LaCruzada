using AppForSEII2526.API.ComprarBonoBocadilloDTOs;

namespace AppForSEII2526.API.ComprarBonoBocadilloDTOs
{
    public class ComprarBonoBocadilloPost
    {
        public ComprarBonoBocadilloPost()
        {
            NombreCliente = string.Empty;
            ApellidoCliente1 = string.Empty;
            ApellidoCliente2 = string.Empty;
            pago=MetodoPago.Tarjeta; // Valor por defecto
            BonoItem = new List<BonosCompradosDTO>();
        }

        public ComprarBonoBocadilloPost(int compraId, string nombreCliente, string apellidoCliente1, string apellidoCliente2,DateTime fechaCompra,MetodoPago metodopago, IList<BonosCompradosDTO> bonoItem)
        {
            CompraId = compraId;
            NombreCliente = nombreCliente ?? string.Empty;
            ApellidoCliente1 = apellidoCliente1 ?? string.Empty;
            ApellidoCliente2 = apellidoCliente2 ?? string.Empty;
            pago = metodopago;
            FechaCompra = fechaCompra;
            BonoItem = bonoItem ?? new List<BonosCompradosDTO>();
        }

        [Key]
        public int CompraId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "El nombre no puede ser más largo a 50 caracteres")]
        public string NombreCliente { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "El apellido no puede ser más largo a 50 caracteres")]
        public string ApellidoCliente1 { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "El apellido no puede ser más largo a 50 caracteres")]
        public string ApellidoCliente2 { get; set; }

        [Required]
        public DateTime FechaCompra { get; set; } = DateTime.Now;

       
        [Required]
        public MetodoPago pago { get; set; }

       
        [Required]
        public IList<BonosCompradosDTO> BonoItem { get; set; }

      
        [JsonPropertyName("precioTotal")]
        public double PrecioTotal
        {
            get
            {
                return BonoItem.Sum(b => b.PrecioUnitario * b.Cantidad);
            }
        }

        public override bool Equals(object? obj)
        {
            if (obj is not ComprarBonoBocadilloPost dTO) return false;

            return CompraId == dTO.CompraId &&
                   string.Equals(NombreCliente, dTO.NombreCliente, StringComparison.Ordinal) &&
                   string.Equals(ApellidoCliente1, dTO.ApellidoCliente1, StringComparison.Ordinal) &&
                   string.Equals(ApellidoCliente2, dTO.ApellidoCliente2, StringComparison.Ordinal) &&
                   FechaCompra.Date == dTO.FechaCompra.Date && // Solo comparar la fecha, no la hora
                   pago== dTO.pago &&
                   PrecioTotal==dTO.PrecioTotal &&
                   BonoItemsEqual(BonoItem, dTO.BonoItem);
        }

        private bool BonoItemsEqual(IList<BonosCompradosDTO>? list1, IList<BonosCompradosDTO>? list2)
        {
            if (list1 == null && list2 == null) return true;
            if (list1 == null || list2 == null) return false;
            if (list1.Count != list2.Count) return false;

            for (int i = 0; i < list1.Count; i++)
            {
                if (!list1[i].Equals(list2[i]))
                    return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CompraId, NombreCliente, ApellidoCliente1, ApellidoCliente2, FechaCompra.Date, pago, PrecioTotal);
        }

        public ApplicationUser ToApplicationUser()
        {
            return new ApplicationUser(NombreCliente, ApellidoCliente1, ApellidoCliente2);
        }
    }
}
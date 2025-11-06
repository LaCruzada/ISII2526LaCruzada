using AppForSEII2526.API.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AppForSEII2526.API.ComprarBonoBocadilloDTOs
{
    public class ComprarBonoBocadilloDetalle
    {
        public ComprarBonoBocadilloDetalle()
        {
            BonosComprados = new List<BonosCompradosDTO>();
        }

        public ComprarBonoBocadilloDetalle(int id, IList<BonosCompradosDTO> bonosComprados, ApplicationUser usuario, MetodoPago pago, DateTime fechaCompra)
        {
            Id = id;
            FechaCompra = fechaCompra;
            BonosComprados = bonosComprados ?? new List<BonosCompradosDTO>();
            MetodoPago = pago;
            if (usuario != null)
            {
                NombreCliente = usuario.Nombre;
                Apellido1 = usuario.Apellido1;
                Apellido2 = usuario.Apellido2;
            }
        }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("fechaCompra")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaCompra { get; set; }

        [Required]
        [ValidateComplexType]
        [JsonPropertyName("bonosComprados")]
        public IList<BonosCompradosDTO> BonosComprados { get; set; }

        [Required]
        [StringLength(50)]
        [JsonPropertyName("nombreCliente")]
        public string NombreCliente { get; set; }

        [Required]
        [StringLength(50)]
        [JsonPropertyName("apellido1")]
        public string Apellido1 { get; set; }

        [StringLength(50)]
        [JsonPropertyName("apellido2")]
        public string Apellido2 { get; set; }

        [JsonPropertyName("apellidos")]
        public string Apellidos 
        { 
            get
            {
                return string.Join(" ", new[] { Apellido1, Apellido2 }
                    .Where(s => !string.IsNullOrWhiteSpace(s)));
            }
        }

        [Required]
        [JsonPropertyName("metodoPago")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public MetodoPago MetodoPago { get; set; }

        [JsonPropertyName("precioTotal")]
        public double PrecioTotal
        {
            get
            {
                return BonosComprados?.Sum(b => b.PrecioUnitario * b.Cantidad) ?? 0;
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is ComprarBonoBocadilloDetalle detalle &&
                   Id == detalle.Id &&
                   FechaCompra == detalle.FechaCompra &&
                   EqualityComparer<IList<BonosCompradosDTO>>.Default.Equals(BonosComprados, detalle.BonosComprados) &&
                   NombreCliente == detalle.NombreCliente &&
                   Apellido1 == detalle.Apellido1 &&
                   Apellido2 == detalle.Apellido2 &&
                   MetodoPago == detalle.MetodoPago;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, FechaCompra, BonosComprados, NombreCliente, Apellido1, Apellido2, MetodoPago);
        }
    }
}

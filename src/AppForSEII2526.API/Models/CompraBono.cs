using System;
using System.ComponentModel;
using DataType = System.ComponentModel.DataAnnotations.DataType;

namespace AppForSEII2526.API.Models
{
    public class CompraBono
    {
        public CompraBono()
        {
        }

        public CompraBono(int compraBonoId, string apellidoBono1, string apellidoBono2, DateTime fechaCompraBono, int nBonos, string nombreCliente, float precioTotalBono)
        {
            CompraBonoId = compraBonoId;
            FechaCompraBono = fechaCompraBono;
            this.nBonos = nBonos;
            PrecioTotalBono = precioTotalBono;
        }

        public CompraBono(int compraBonoId, DateTime fechaCompraBono, int nBonos, float precioTotalBono, MetodoPago metodoPago, List<BonosComprados> bonosComprados, List<ApplicationUser> usuario)
        {
            CompraBonoId = compraBonoId;
            FechaCompraBono = fechaCompraBono;
            this.nBonos = nBonos;
            PrecioTotalBono = precioTotalBono;
            this.metodoPago = metodoPago;
            this.bonosComprados = bonosComprados;
            this.usuario = usuario;
        }

        [Key]
        public int CompraBonoId {  get; set; }
      

        [DataType(DataType.Date), Display(Name = "Fecha Bono")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaCompraBono {  get; set; }

        [Required]        
        public int nBonos { get; set; }

        [Required]
        public float PrecioTotalBono { get; set; } = 0f;

        public MetodoPago metodoPago { get; set; }

        public List<BonosComprados> bonosComprados { get; set; } = new List<BonosComprados>();

        public List<ApplicationUser> usuario { get; set;  } = new List<ApplicationUser>();

        public override bool Equals(object? obj)
        {
            return obj is CompraBono bono &&
                   CompraBonoId == bono.CompraBonoId &&
                   FechaCompraBono == bono.FechaCompraBono &&
                   nBonos == bono.nBonos &&
                   PrecioTotalBono == bono.PrecioTotalBono &&
                   metodoPago == bono.metodoPago &&
                   EqualityComparer<List<BonosComprados>>.Default.Equals(bonosComprados, bono.bonosComprados) &&
                   EqualityComparer<List<ApplicationUser>>.Default.Equals(usuario, bono.usuario);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CompraBonoId, FechaCompraBono, nBonos, PrecioTotalBono, metodoPago, bonosComprados, usuario);
        }
    }
}

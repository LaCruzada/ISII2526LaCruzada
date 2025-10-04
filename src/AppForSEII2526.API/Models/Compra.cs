using System;
using System.ComponentModel;
using DataType = System.ComponentModel.DataAnnotations.DataType;

namespace AppForSEII2526.API.Models
{
    public class Compra
    {
       

        [Key]
        public int CompraId { get; set; }


        [DataType(DataType.Date), Display(Name = "Fecha Bono")]
        public DateTime FechaCompra { get; set; }

        [Required]
        public int nBocadillos { get; set; }

        [Required]
        public float PrecioTotal { get; set; } = 0f;

        public MetodoPago MetodoPago { get; set; }

        public List<CompraBocadillo> BocadillosComprados { get; set; } = new List<CompraBocadillo>();

        public List<ApplicationUser> Cliente { get; set; } = new List<ApplicationUser>();

        
        public Compra(int compraId, DateTime fechaCompra, int nBocadillos, float precioTotal, MetodoPago metodoPago, List<ApplicationUser> usuario, List<CompraBocadillo> BocadillosComprados)
        {
            CompraId = compraId;
            FechaCompra = fechaCompra;
            this.nBocadillos = nBocadillos;
            PrecioTotal = precioTotal;
            this.MetodoPago = metodoPago;
            this.Cliente = usuario;
            BocadillosComprados = BocadillosComprados;
        }

        public override bool Equals(object? obj)
        {
            return obj is Compra compra &&
                   CompraId == compra.CompraId &&
                   FechaCompra == compra.FechaCompra &&
                   nBocadillos == compra.nBocadillos &&
                   PrecioTotal == compra.PrecioTotal &&
                   MetodoPago == compra.MetodoPago &&
                   EqualityComparer<List<CompraBocadillo>>.Default.Equals(BocadillosComprados, compra.BocadillosComprados) &&
                   EqualityComparer<List<ApplicationUser>>.Default.Equals(Cliente, compra.Cliente);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CompraId, FechaCompra, nBocadillos, PrecioTotal, MetodoPago, BocadillosComprados, Cliente);
        }
    }
}
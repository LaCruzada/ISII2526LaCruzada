using System;
using System.ComponentModel;
using DataType = System.ComponentModel.DataAnnotations.DataType;

namespace AppForSEII2526.API.Models
{
    public class Compra_Producto
    {
        public Compra_Producto()
        {
            ProductoCompras = new List<ProductoCompra>();
            Nombre = string.Empty;
            Apellido_1 = string.Empty;
            Apellido_2 = string.Empty;
            DireccionEnvio = string.Empty;
            Metodo_Pago = string.Empty;
        }

        public Compra_Producto(int compraId, string nombre, string apellido1, string apellido2,
                     string direccionEnvio, DateTime fechaCompra, string metodoPago, decimal precioFinal)
        {
            CompraId = compraId;
            Nombre = nombre;
            Apellido_1 = apellido1;
            Apellido_2 = apellido2;
            DireccionEnvio = direccionEnvio;
            FechaCompra = fechaCompra;
            Metodo_Pago = metodoPago;
            PrecioFinal = precioFinal;
            ProductoCompras = new List<ProductoCompra>();
        }

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
            return obj is Compra_Producto compra &&
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
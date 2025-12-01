using System;
using System.Collections.Generic;
using System.Linq;

namespace AppForSEII2526.API.DTOs.DTOsPedirBocadillo
{
    public class PedidoBocadilloDetailsDTO
    {
        public int PedidoId { get; set; }
        public string NombreCliente { get; set; }
        public DateTime FechaCompra { get; set; }
        public string MetodoPago { get; set; }
        public decimal PrecioTotal { get; set; }
        public List<BocadilloItemDTO> Bocadillos { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is PedidoBocadilloDetailsDTO dto &&
                   PedidoId == dto.PedidoId &&
                   NombreCliente == dto.NombreCliente &&
                   FechaCompra == dto.FechaCompra &&
                   MetodoPago == dto.MetodoPago &&
                   PrecioTotal == dto.PrecioTotal &&
                   (Bocadillos != null && dto.Bocadillos != null && Bocadillos.SequenceEqual(dto.Bocadillos));
            
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PedidoId, NombreCliente, FechaCompra, MetodoPago, PrecioTotal);
        }
    }
}
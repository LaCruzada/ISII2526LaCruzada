using AppForSEII2526.API.DTOs.DTOsPedirBocadillo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppForSEII2526.Web.Services
{
    public class PedirBocadilloStateContainer
    {

        public PedirBocadilloCreateDTO Pedido { get; private set; } = new PedirBocadilloCreateDTO()
        {
            Bocadillos = new List<BocadilloPedidoItemDTO>(),
            NombreCliente = "",
            Apellido1Cliente = "",
            EmailCliente = "",
            MetodoPago = "Tarjeta"
        };

        public List<BocadilloSelectDTO> CarritoVisual { get; private set; } = new List<BocadilloSelectDTO>();

        public decimal PrecioTotal
        {
            get
            {
                return CarritoVisual.Sum(b => b.Precio);
            }
        }


        public event Action? OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();

        public void AgregarBocadillo(BocadilloSelectDTO bocadilloSeleccionado)
        {

            CarritoVisual.Add(bocadilloSeleccionado);

            var itemExistente = Pedido.Bocadillos.FirstOrDefault(b => b.BocadilloId == bocadilloSeleccionado.Id);

            if (itemExistente != null)
            {

                itemExistente.Cantidad++;
            }
            else
            {
                Pedido.Bocadillos.Add(new BocadilloPedidoItemDTO
                {
                    BocadilloId = bocadilloSeleccionado.Id,
                    Cantidad = 1
                });
            }

            NotifyStateChanged();
        }

        public void EliminarBocadillo(BocadilloSelectDTO bocadilloSeleccionado)
        {

            CarritoVisual.Remove(bocadilloSeleccionado);


            var itemExistente = Pedido.Bocadillos.FirstOrDefault(b => b.BocadilloId == bocadilloSeleccionado.Id);

            if (itemExistente != null)
            {
                itemExistente.Cantidad--;
                if (itemExistente.Cantidad <= 0)
                {
                    Pedido.Bocadillos.Remove(itemExistente);
                }
            }

            NotifyStateChanged();
        }

        public void LimpiarCarrito()
        {
            Pedido.Bocadillos.Clear();
            CarritoVisual.Clear();
            NotifyStateChanged();
        }

        public void PedidoProcesado()
        {
            Pedido = new PedirBocadilloCreateDTO()
            {
                Bocadillos = new List<BocadilloPedidoItemDTO>(),
                NombreCliente = "",
                Apellido1Cliente = "",
                EmailCliente = "",
                MetodoPago = "Tarjeta"
            };
            CarritoVisual = new List<BocadilloSelectDTO>();

            NotifyStateChanged();
        }
    }
}
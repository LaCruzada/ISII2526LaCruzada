using AppForSEII2526.API.DTOs.DTOsCompraMerchandising;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppForSEII2526.Web.Services
{
    /// <summary>
    /// State container que mantiene el carrito de merchandising entre páginas
    /// y notifica cambios a la UI.
    /// </summary>
    public class MerchandisingStateContainer
    {

        public ComprarMerchandisingCreateDTO Compra { get; private set; } = new()
        {
            Productos = new List<ProductoCantidadDTO>(),
            NombreCliente = string.Empty,
            Apellido1Cliente = string.Empty,
            Apellido2Cliente = string.Empty,
            DireccionEnvio = string.Empty,
            MetodoPago = "TarjetaCredito"
        };

        public List<ProductoSelectDTO> CarritoVisual { get; private set; } = new();

        public decimal PrecioTotal => CarritoVisual.Sum(p => p.Precio);

        public event Action? OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();
        public void AgregarProducto(ProductoSelectDTO producto)
        {
            CarritoVisual.Add(producto);

            var itemExistente = Compra.Productos.FirstOrDefault(i => i.ProductoId == producto.Id);

            if (itemExistente != null)
                itemExistente.Cantidad++;
            else
                Compra.Productos.Add(new ProductoCantidadDTO
                {
                    ProductoId = producto.Id,
                    Cantidad = 1
                });

            NotifyStateChanged();
        }
        public void EliminarProducto(ProductoSelectDTO producto)
        {

            var primero = CarritoVisual.First(p => p.Id == producto.Id);
            CarritoVisual.Remove(primero);


            var item = Compra.Productos.FirstOrDefault(i => i.ProductoId == producto.Id);
            if (item == null) return;

            item.Cantidad--;
            if (item.Cantidad <= 0)
                Compra.Productos.Remove(item);

            NotifyStateChanged();
        }

        public void LimpiarCarrito()
        {
            Compra.Productos.Clear();
            CarritoVisual.Clear();
            NotifyStateChanged();
        }

        public void CompraProcesada()
        {
            Compra = new()
            {
                Productos = new List<ProductoCantidadDTO>(),
                NombreCliente = string.Empty,
                Apellido1Cliente = string.Empty,
                Apellido2Cliente = string.Empty,
                DireccionEnvio = string.Empty,
                MetodoPago = "TarjetaCredito"
            };
            CarritoVisual = new();
            NotifyStateChanged();
        }
    }
}
using AppForSEII2526.API.ComprarBonoBocadilloDTOs;
using AppForSEII2526.API.DTOs.CompraBonoDTOs;
using AppForSEII2526.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppForSEII2526.Web.Services
{
    public class ComprarBonoStateContainer
    {
        public ComprarBonoBocadilloPost Compra { get; private set; } = new ComprarBonoBocadilloPost
        {
            NombreCliente = "",
            ApellidoCliente1 = "",
            ApellidoCliente2 = "",
            pago = MetodoPago.Tarjeta,
            FechaCompra = DateTime.Now,
            BonoItem = new List<BonosCompradosDTO>()
        };

        public List<BonoSelectDTO> CarritoVisual { get; private set; } = new List<BonoSelectDTO>();

        public double PrecioTotal => Compra.PrecioTotal;

        public event Action? OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();

        public void AgregarBono(BonoSelectDTO bonoSeleccionado)
        {
            if (bonoSeleccionado == null) return;

            var itemExistente = Compra.BonoItem.FirstOrDefault(b => b.BonoID == bonoSeleccionado.BonoId);

            // Si ya existe, sólo incrementar si no se ha alcanzado el stock disponible
            if (itemExistente != null)
            {
                if (itemExistente.Cantidad < bonoSeleccionado.CantidadDisponible)
                {
                    itemExistente.Cantidad++;
                }
                else
                {
                    // No hay más unidades disponibles; no hacemos nada
                    return;
                }
            }
            else
            {
                // Nuevo item: sólo añadir si hay stock
                if (bonoSeleccionado.CantidadDisponible > 0)
                {
                    Compra.BonoItem.Add(new BonosCompradosDTO
                    {
                        BonoID = bonoSeleccionado.BonoId,
                        Nombre = bonoSeleccionado.Nombre,
                        PrecioUnitario = bonoSeleccionado.PVP,
                        Cantidad = 1,
                        Tipo = bonoSeleccionado.TipoBocadillo
                    });

                    // Añadir a la representación visual si no existe ya
                    if (!CarritoVisual.Any(b => b.BonoId == bonoSeleccionado.BonoId))
                    {
                        CarritoVisual.Add(bonoSeleccionado);
                    }
                }
                else
                {
                    // Sin stock, nada que hacer
                    return;
                }
            }

            NotifyStateChanged();
        }

        public void EliminarBono(BonoSelectDTO bonoSeleccionado)
        {
            if (bonoSeleccionado == null) return;

            var itemExistente = Compra.BonoItem.FirstOrDefault(b => b.BonoID == bonoSeleccionado.BonoId);

            if (itemExistente != null)
            {
                itemExistente.Cantidad--;
                if (itemExistente.Cantidad <= 0)
                {
                    Compra.BonoItem.Remove(itemExistente);
                    var visual = CarritoVisual.FirstOrDefault(b => b.BonoId == bonoSeleccionado.BonoId);
                    if (visual != null) CarritoVisual.Remove(visual);
                }
            }

            NotifyStateChanged();
        }

        public void LimpiarCarrito()
        {
            Compra.BonoItem.Clear();
            CarritoVisual.Clear();
            NotifyStateChanged();
        }

        public void CompraProcesada()
        {
            Compra = new ComprarBonoBocadilloPost
            {
                NombreCliente = "",
                ApellidoCliente1 = "",
                ApellidoCliente2 = "",
                pago = MetodoPago.Tarjeta,
                FechaCompra = DateTime.Now,
                BonoItem = new List<BonosCompradosDTO>()
            };
            CarritoVisual = new List<BonoSelectDTO>();

            NotifyStateChanged();
        }

        public void ActualizarMetodoPago(MetodoPago metodoPago)
        {
            Compra.pago = metodoPago;
            NotifyStateChanged();
        }

        public void ActualizarDatosCliente(string nombre, string apellido1, string apellido2)
        {
            Compra.NombreCliente = nombre;
            Compra.ApellidoCliente1 = apellido1;
            Compra.ApellidoCliente2 = apellido2;
            NotifyStateChanged();
        }
    }
}
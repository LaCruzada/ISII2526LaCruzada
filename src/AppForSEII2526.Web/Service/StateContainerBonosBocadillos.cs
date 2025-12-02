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
            CarritoVisual.Add(bonoSeleccionado);

            var itemExistente = Compra.BonoItem.FirstOrDefault(b => b.BonoID == bonoSeleccionado.BonoId);

            if (itemExistente != null)
            {
                itemExistente.Cantidad++;
            }
            else
            {
                Compra.BonoItem.Add(new BonosCompradosDTO
                {
                    BonoID = bonoSeleccionado.BonoId,
                    Nombre = bonoSeleccionado.Nombre,
                    PrecioUnitario = bonoSeleccionado.PVP,
                    Cantidad = 1,
                    Tipo = bonoSeleccionado.TipoBocadillo
                });
            }

            NotifyStateChanged();
        }

        public void EliminarBono(BonoSelectDTO bonoSeleccionado)
        {
            CarritoVisual.Remove(bonoSeleccionado);

            var itemExistente = Compra.BonoItem.FirstOrDefault(b => b.BonoID == bonoSeleccionado.BonoId);

            if (itemExistente != null)
            {
                itemExistente.Cantidad--;
                if (itemExistente.Cantidad <= 0)
                {
                    Compra.BonoItem.Remove(itemExistente);
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
    }


}
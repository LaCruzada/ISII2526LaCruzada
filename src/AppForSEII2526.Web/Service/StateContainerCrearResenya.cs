using AppForSEII2526.Web.API;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppForSEII2526.Web.Services
{
    public class StateContainerCrearResenya
    {
        public ResenyaForCreacionDTO Resenya { get; private set; } = new ResenyaForCreacionDTO
        {
            NombreUsuario = "",
            Titulo = "",
            Descripcion = "",
            ValoracionGeneral = EnumValoracion_General.Una,
            ResenyaBocadillos = new List<ResenyaBocadilloDTO>(),
            FechaPublicacion = DateTime.Now
        };

        public event Action? OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();

        public void AgregarBocadilloParaResenya(AppForSEII2526.API.DTOs.DTOsCrearResenya.BocadillosDTOs.BocadilloSeleccionadoDTO bocadillo)
        {
            if (!Resenya.ResenyaBocadillos.Any(b => b.BocadilloId == bocadillo.Id))
            {
                Resenya.ResenyaBocadillos.Add(new ResenyaBocadilloDTO
                {
                    BocadilloId = bocadillo.Id,
                    Nombre = bocadillo.Nombre,
                    Pvp = (double)bocadillo.PVP,
                    Tamano = (EnumTamaño)bocadillo.Tamano,
                    Puntuacion = 1
                });
                NotifyStateChanged();
            }
        }

        public void EliminarBocadilloParaResenya(AppForSEII2526.API.DTOs.DTOsCrearResenya.BocadillosDTOs.BocadilloSeleccionadoDTO bocadillo)
        {
            if (Resenya.ResenyaBocadillos.Any(b => b.BocadilloId == bocadillo.Id))
            {
                var itemAEliminar = Resenya.ResenyaBocadillos.First(b => b.BocadilloId == bocadillo.Id);
                Resenya.ResenyaBocadillos.Remove(itemAEliminar);
                NotifyStateChanged();
            }
        }

        public void LimpiarCarritoResenya()
        {
            Resenya.ResenyaBocadillos.Clear();
            NotifyStateChanged();
        }

        public void ActualizarDatosResenya(string? nombreUsuario, string titulo, string descripcion, EnumValoracion_General valoracionGeneral)
        {
            Resenya.NombreUsuario = nombreUsuario;
            Resenya.Titulo = titulo;
            Resenya.Descripcion = descripcion;
            Resenya.ValoracionGeneral = valoracionGeneral;
            NotifyStateChanged();
        }

        public void ActualizarPuntuacionBocadillo(int bocadilloId, int puntuacion)
        {
            var bocadillo = Resenya.ResenyaBocadillos.FirstOrDefault(b => b.BocadilloId == bocadilloId);
            if (bocadillo != null)
            {
                bocadillo.Puntuacion = puntuacion;
                NotifyStateChanged();
            }
        }

        public void ResenyaProcesada()
        {
            Resenya = new ResenyaForCreacionDTO()
            {
                ResenyaBocadillos = new List<ResenyaBocadilloDTO>(),
                FechaPublicacion = DateTime.Now,
                ValoracionGeneral = EnumValoracion_General.Una
            };
            NotifyStateChanged();
        }
    }
}

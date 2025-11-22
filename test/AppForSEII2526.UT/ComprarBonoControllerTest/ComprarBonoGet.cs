using AppForMovies.UT;
using AppForSEII2526.API.ComprarBonoBocadilloDTOs;
using AppForSEII2526.API.Controllers.ControllerComprarBono;
using AppForSEII2526.API.DTOs.CompraBonoDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.ComprarBonoGet
{
    public class ComprarBonoGet: AppForMovies4SqliteUT
    {
        private int _existingCompraId;
        private ApplicationUser _cliente;
        private MetodoPago _metodoPago;
        private List<BonosCompradosDTO> _expectedItems;
        private double _expectedPrecioTotal;
        private DateTime _fechaCompra;

        public ComprarBonoGet()
        {
            // Tipo de bono
            var tipo = new TipoBocadillo { nombreTipo= "Normal" };

            // Bono
            var bono = new BonoBocadillo
            {
                BonoId = 1,
                nombre = "BonoTest",
                PVP = 10.0,
                nBocadillos = 2,
                cantidadDisponible = 5,
                tipoBocadillos = tipo
            };

            _metodoPago = MetodoPago.Tarjeta;

            // Usuario
            _cliente = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                Nombre = "Cliente1",
                Apellido1 = "Ap1",
                Apellido2 = "Ap2",
                UserName = "cliente1"
            };

            // Crear compra y linea de compra (2 unidades)
            _fechaCompra = DateTime.Now;
            var compra = new CompraBono
            {
                FechaCompraBono = _fechaCompra,
                metodoPago = _metodoPago,
               usuarios = new List<ApplicationUser> { _cliente },
                nBonos = 2,
                PrecioTotalBono = 20.0f
            };

            var bonosComprados = new BonosComprados
            {
                BonoId = bono.BonoId,
                Cantidad = 2,
                PrecioBono = (float)bono.PVP,
                Bono = bono,
                Compra = compra
            };

            compra.bonosComprados.Add(bonosComprados);

            // Añadir al contexto
            _context.TipoBocadillos.Add(tipo);
            _context.BonoBocadillo.Add(bono);
            _context.usuarios.Add(_cliente);
            _context.CompraBono.Add(compra);
            _context.SaveChanges();

            // Guardar Id generado
            _existingCompraId = compra.CompraBonoId;

            // Datos esperados para las aserciones
            _expectedPrecioTotal = compra.PrecioTotalBono;
            _expectedItems = compra.bonosComprados
                .Select(bc => new BonosCompradosDTO(bc.Bono.BonoId,bc.Bono.nombre, bc.PrecioBono, bc.Bono.nBocadillos, bc.Bono.tipoBocadillos.nombreTipo))
                .ToList();
        }

        [Fact]
        public async Task GetCompra_NotFound_test()
        {
            // Arrange
            var mock = new Mock<ILogger<CompraBonoController>>();
            var controller = new CompraBonoController(_context, mock.Object);

            // Act
            var result = await controller.GetCompra(0);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetCompra_Found_test()
        {
            // Arrange
            var mock = new Mock<ILogger<CompraBonoController>>();
            var controller = new CompraBonoController(_context, mock.Object);

            // Construimos un DTO esperado sencillo (estilo "details" simple)
            var expected = new ComprarBonoBocadilloDetalle(_existingCompraId, _cliente, _metodoPago, _fechaCompra, _expectedPrecioTotal, _expectedItems);

            // Act
            var result = await controller.GetCompra(_existingCompraId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actual = Assert.IsType<ComprarBonoBocadilloDetalle>(okResult.Value);

            // Comparación directa como en el ejemplo simplificado solicitado
            Assert.Equal(expected, actual);
        }
    }
}
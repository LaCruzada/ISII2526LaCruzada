using AppForSEII2526.API.Controllers.ControllerPedirBocadillo;
using AppForSEII2526.API.Data;
using AppForSEII2526.API.DTOs.DTOsPedirBocadillo;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AppForSEII2526.UT.BocadilloController_test
{
    public class PedirBocadilloControllerGetDetailsTEST : AppForMovies.UT.AppForMovies4SqliteUT
    {
        private readonly ApplicationUser _testUser;
        private readonly Compra _testCompra;
        private readonly Bocadillo _testBocadillo;
        private readonly TipoPan _testTipoPan;

        public PedirBocadilloControllerGetDetailsTEST()
        {
            _testUser = new ApplicationUser
            {
                Id = "user-test-id",
                Nombre = "Juan",
                Apellido1 = "Perez",
                Apellido2 = "Garcia",
                Email = "juan.perez@ejemplo.com",
                UserName = "juan.perez@ejemplo.com",
                EmailConfirmed = true
            };
            _context.Users.Add(_testUser);

            _testTipoPan = new TipoPan { PanId = 1, Nombre = "Barra" };
            _context.TipoPanes.Add(_testTipoPan);

            _testBocadillo = new Bocadillo
            {
                Id = 1,
                Nombre = "Bocata de Prueba",
                PVP = 5.50m,
                Stock = 10,
                TipoPan = _testTipoPan,
                Tamano = EnumTamaño.Normal
            };
            _context.Bocadillos.Add(_testBocadillo);

            _testCompra = new Compra
            {
                CompraId = 1,
                FechaCompra = DateTime.Now,
                MetodoPago = MetodoPago.Tarjeta,
                PrecioTotal = 11.00f,
                nBocadillos = 2,
                Cliente = new List<ApplicationUser> { _testUser },
                BocadillosComprados = new List<CompraBocadillo>()
            };
            _context.Compra.Add(_testCompra);

            var compraBocadillo = new CompraBocadillo
            {
                CompraId = _testCompra.CompraId,
                BocadilloId = _testBocadillo.Id,
                Cantidad = 2,
                Bocadillo = _testBocadillo,
                Compra = _testCompra
            };
            _context.CompraBocadillo.Add(compraBocadillo);

            _context.SaveChanges();
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetPedido_Muestre_Success_test()
        {
            var controller = new PedirBocadilloController(_context);
            var idSolicitado = 1;

            var expectedDto = new PedidoBocadilloDetailsDTO
            {
                PedidoId = _testCompra.CompraId,
                NombreCliente = $"{_testUser.Nombre} {_testUser.Apellido1} {_testUser.Apellido2}".Trim(),
                FechaCompra = _testCompra.FechaCompra,
                MetodoPago = _testCompra.MetodoPago.ToString(),
                PrecioTotal = (decimal)_testCompra.PrecioTotal, 
                Bocadillos = new List<BocadilloItemDTO>
                {
                    new BocadilloItemDTO
                    {
                        BocadilloId = _testBocadillo.Id,
                        NombreBocadillo = _testBocadillo.Nombre,
                        TipoPan = _testTipoPan.Nombre,
                        Cantidad = 2,
                        PrecioUnitario = _testBocadillo.PVP
                    }
                }
            };

            var actionResult = await controller.GetPedido(idSolicitado);

            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var actualDto = Assert.IsType<PedidoBocadilloDetailsDTO>(okResult.Value);
            expectedDto.FechaCompra = actualDto.FechaCompra;
            Assert.Equal(expectedDto, actualDto);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetPedido_NoMuestre_DevuelveBadRequest()
        {
            var controller = new PedirBocadilloController(_context);
            var nonExistentId = 999;

            var actionResult = await controller.GetPedido(nonExistentId);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.NotNull(badRequestResult.Value);
        }
    }
}
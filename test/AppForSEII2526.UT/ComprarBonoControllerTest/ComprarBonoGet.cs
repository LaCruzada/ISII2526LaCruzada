/*
using AppForSEII2526.API.ComprarBonoBocadilloDTOs;
using AppForSEII2526.API.Controllers.ControllerComprarBono;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AppForSEII2526.UT.ComprarBonoControllerTest
{
    public class ComprarBonoGet : AppForMovies.UT.AppForMovies4SqliteUT
    {
        private readonly ApplicationUser _testUser;
        private readonly CompraBono _testCompraBono;
        private readonly BonoBocadillo _testBono;
        private readonly TipoBocadillo _testTipoBocadillo;

        public ComprarBonoGet()
        {
            // Crear usuario de prueba
            _testUser = new ApplicationUser
            {
                Id = "user-test-id",
                Nombre = "Juan",
                Apellido1 = "Pérez",
                Apellido2 = "García",
                Email = "juan.perez@ejemplo.com",
                UserName = "juan.perez@ejemplo.com",
                EmailConfirmed = true
            };
            _context.Users.Add(_testUser);


            _testTipoBocadillo = new TipoBocadillo(1, "Vegetal", new List<BonoBocadillo>());
            _context.TipoBocadillos.Add(_testTipoBocadillo);


            _testBono = new BonoBocadillo(1, 10, 5, "Bono Vegetal 5", 25.0f, _testTipoBocadillo, new List<BonosComprados>());
            _context.BonoBocadillo.Add(_testBono);


            _testCompraBono = new CompraBono(
                1,
                DateTime.Today,
                2,
                50.0f,
                MetodoPago.Tarjeta,
                new List<BonosComprados>(),
                new List<ApplicationUser> { _testUser }
            );
            _context.CompraBono.Add(_testCompraBono);


            var bonoComprado = new BonosComprados
            {
                CompraId = _testCompraBono.CompraBonoId,
                BonoId = _testBono.BonoId,
                Cantidad = 2,
                PrecioBono = 25.0f,
                Bono = _testBono,
                Compra = _testCompraBono
            };

            _testCompraBono.bonosComprados.Add(bonoComprado);

            _context.SaveChanges();
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetBonoCompra_Existe_Success_test()
        {
            // Arrange
            var mock = new Mock<ILogger<CompraBonoController>>();
            ILogger<CompraBonoController> logger = mock.Object;
            var controller = new CompraBonoController(_context, logger);

            var expectedId = 1;
            var expectedNombre = _testUser.Nombre;
            var expectedApellido1 = _testUser.Apellido1;
            var expectedApellido2 = _testUser.Apellido2;

            // Act
            var actionResult = await controller.GetBonoCompra(expectedId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            var dto = Assert.IsType<ComprarBonoBocadilloDetalle>(okResult.Value);

            Assert.Equal(expectedId, dto.Id);
            Assert.Equal(expectedNombre, dto.NombreCliente);
            Assert.Equal(expectedApellido1, dto.Apellido1);
            Assert.Equal(expectedApellido2, dto.Apellido2);
            Assert.Single(dto.BonosComprados);
            Assert.Equal(_testBono.nombre, dto.BonosComprados.First().Nombre);
            Assert.Equal(2, dto.BonosComprados.First().Cantidad);
            Assert.Equal(25.0, dto.BonosComprados.First().PrecioUnitario);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetBonoCompra_NoExiste_DevuelveNotFound()
        {
            // Arrange
            var mock = new Mock<ILogger<CompraBonoController>>();
            ILogger<CompraBonoController> logger = mock.Object;
            var controller = new CompraBonoController(_context, logger);
            var nonExistentId = 999;

            // Act
            var actionResult = await controller.GetBonoCompra(nonExistentId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(actionResult);
            Assert.NotNull(notFoundResult);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetBonoCompra_VerificarFechaCompra_Success_test()
        {
            // Arrange
            var mock = new Mock<ILogger<CompraBonoController>>();
            ILogger<CompraBonoController> logger = mock.Object;
            var controller = new CompraBonoController(_context, logger);
            var expectedId = 1;

            // Act
            var actionResult = await controller.GetBonoCompra(expectedId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            var dto = Assert.IsType<ComprarBonoBocadilloDetalle>(okResult.Value);

            Assert.Equal(DateTime.Today, dto.FechaCompra.Date);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetBonoCompra_VerificarMetodoPago_Success_test()
        {
            // Arrange
            var mock = new Mock<ILogger<CompraBonoController>>();
            ILogger<CompraBonoController> logger = mock.Object;
            var controller = new CompraBonoController(_context, logger);
            var expectedId = 1;

            // Act
            var actionResult = await controller.GetBonoCompra(expectedId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            var dto = Assert.IsType<ComprarBonoBocadilloDetalle>(okResult.Value);

            Assert.Equal(MetodoPago.Tarjeta, dto.MetodoPago);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetBonoCompra_VerificarPrecioTotal_Success_test()
        {
            // Arrange
            var mock = new Mock<ILogger<CompraBonoController>>();
            ILogger<CompraBonoController> logger = mock.Object;
            var controller = new CompraBonoController(_context, logger);
            var expectedId = 1;

            // Act
            var actionResult = await controller.GetBonoCompra(expectedId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            var dto = Assert.IsType<ComprarBonoBocadilloDetalle>(okResult.Value);

            Assert.Equal(50.0, dto.PrecioTotal);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetBonoCompra_VerificarApellidos_Success_test()
        {
            // Arrange
            var mock = new Mock<ILogger<CompraBonoController>>();
            ILogger<CompraBonoController> logger = mock.Object;
            var controller = new CompraBonoController(_context, logger);
            var expectedId = 1;
            var expectedApellidos = $"{_testUser.Apellido1} {_testUser.Apellido2}".Trim();

            // Act
            var actionResult = await controller.GetBonoCompra(expectedId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            var dto = Assert.IsType<ComprarBonoBocadilloDetalle>(okResult.Value);

            Assert.Equal(expectedApellidos, dto.Apellidos);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetBonoCompra_VerificarBonoDTO_Success_test()
        {
            // Arrange
            var mock = new Mock<ILogger<CompraBonoController>>();
            ILogger<CompraBonoController> logger = mock.Object;
            var controller = new CompraBonoController(_context, logger);
            var expectedId = 1;

            // Act
            var actionResult = await controller.GetBonoCompra(expectedId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            var dto = Assert.IsType<ComprarBonoBocadilloDetalle>(okResult.Value);

            var bonoDTO = dto.BonosComprados.First();
            Assert.Equal(1, bonoDTO.BonoID);
            Assert.Equal("Bono Vegetal 5", bonoDTO.Nombre);
            Assert.Equal(25.0, bonoDTO.PrecioUnitario);
            Assert.Equal(2, bonoDTO.Cantidad);
            Assert.Equal("Vegetal", bonoDTO.Tipo);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetBonoCompra_CompraConVariosBonosComprados_Success_test()
        {
            // Arrange
            var mock = new Mock<ILogger<CompraBonoController>>();
            ILogger<CompraBonoController> logger = mock.Object;

            // Crear un segundo bono
            var tipoBocadillo2 = new TipoBocadillo(2, "Mixto", new List<BonoBocadillo>());
            _context.TipoBocadillos.Add(tipoBocadillo2);

            var bono2 = new BonoBocadillo(2, 15, 10, "Bono Mixto 10", 45.0f, tipoBocadillo2, new List<BonosComprados>());
            _context.BonoBocadillo.Add(bono2);

            var compra2 = new CompraBono(
                2,
                DateTime.Today,
                3,
                95.0f,
                MetodoPago.Paypal,
                new List<BonosComprados>(),
                new List<ApplicationUser> { _testUser }
            );
            _context.CompraBono.Add(compra2);

            var bonoComprado1 = new BonosComprados
            {
                CompraId = compra2.CompraBonoId,
                BonoId = _testBono.BonoId,
                Cantidad = 2,
                PrecioBono = 25.0f,
                Bono = _testBono,
                Compra = compra2
            };

            var bonoComprado2 = new BonosComprados
            {
                CompraId = compra2.CompraBonoId,
                BonoId = bono2.BonoId,
                Cantidad = 1,
                PrecioBono = 45.0f,
                Bono = bono2,
                Compra = compra2
            };

            compra2.bonosComprados.Add(bonoComprado1);
            compra2.bonosComprados.Add(bonoComprado2);

            _context.SaveChanges();

            var controller = new CompraBonoController(_context, logger);

            // Act
            var actionResult = await controller.GetBonoCompra(2);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            var dto = Assert.IsType<ComprarBonoBocadilloDetalle>(okResult.Value);

            Assert.Equal(2, dto.Id);
            Assert.Equal(2, dto.BonosComprados.Count);
            Assert.Equal(95.0, dto.PrecioTotal);
        }
    }
}
*/
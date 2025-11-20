using AppForSEII2526.API.ComprarBonoBocadilloDTOs;
using AppForSEII2526.API.Controllers;
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
    public class ComprarBonoPost : AppForMovies.UT.AppForMovies4SqliteUT
    {
        private const int BONO_ID_VALIDO = 1;
        private const string BONO_NOMBRE_VALIDO = "Bono Vegetal 5";
        private const int BONO_STOCK_INICIAL = 10;
        private const float BONO_PVP_VALIDO = 25.0f;

        private const int BONO_ID_POCO_STOCK = 2;
        private const string BONO_NOMBRE_POCO_STOCK = "Bono Mixto Raro";
        private const int BONO_STOCK_INICIAL_POCO = 2;
        private const float BONO_PVP_POCO_STOCK = 40.0f;

        private const int ID_NO_EXISTENTE = 99;

        public ComprarBonoPost()
        {
            var tipoVegetal = new TipoBocadillo(1, "Vegetal", new List<BonoBocadillo>());
            var tipoMixto = new TipoBocadillo(2, "Mixto", new List<BonoBocadillo>());

            _context.TipoBocadillos.AddRange(tipoVegetal, tipoMixto);

            _context.BonoBocadillo.AddRange(
                new BonoBocadillo(
                    BONO_ID_VALIDO,
                    BONO_STOCK_INICIAL,
                    5,
                    BONO_NOMBRE_VALIDO,
                    BONO_PVP_VALIDO,
                    tipoVegetal,
                    new List<BonosComprados>()
                ),
                new BonoBocadillo(
                    BONO_ID_POCO_STOCK,
                    BONO_STOCK_INICIAL_POCO,
                    10,
                    BONO_NOMBRE_POCO_STOCK,
                    BONO_PVP_POCO_STOCK,
                    tipoMixto,
                    new List<BonosComprados>()
                )
            );

            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_CrearCompraBono_CosasObligatorias()
        {
            var dtoBonoNoExiste = new ComprarBonoBocadilloPost
            {
                usuario = new UsuarioCompraDTO
                {
                    Nombre = "Test",
                    Apellido1 = "Test",
                    Apellido2 = "T"
                },
                MetodoPago = MetodoPago.Tarjeta,
                BonosCompra = new List<BonosCompradosDTO>
                {
                    new BonosCompradosDTO(ID_NO_EXISTENTE, "Bono Inexistente", 25.0, 1, "Vegetal")
                }
            };

            var dtoSinStock = new ComprarBonoBocadilloPost
            {
                usuario = new UsuarioCompraDTO
                {
                    Nombre = "Test",
                    Apellido1 = "Test",
                    Apellido2 = "T"
                },
                MetodoPago = MetodoPago.Tarjeta,
                BonosCompra = new List<BonosCompradosDTO>
                {
                    new BonosCompradosDTO(BONO_ID_POCO_STOCK, BONO_NOMBRE_POCO_STOCK, BONO_PVP_POCO_STOCK, 5, "Mixto")
                }
            };

            var allTests = new List<object[]>
            {
                new object[] { dtoBonoNoExiste, $"Error! El bono con nombre Bono Inexistente y con ID {ID_NO_EXISTENTE} no existe en la base de datos." },
                new object[] { dtoSinStock, $"Error! El bono con nombre {BONO_NOMBRE_POCO_STOCK} solo tiene {BONO_STOCK_INICIAL_POCO} unidades disponibles, pero has seleccionado 5 unidades para comprar." },
            };

            return allTests;
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        [MemberData(nameof(TestCasesFor_CrearCompraBono_CosasObligatorias))]
        public async Task CrearCompraBono_CosasObligatorias_DevuelveBadRequest(ComprarBonoBocadilloPost dtoConError, string errorEsperado)
        {
            // Arrange
            var mock = new Mock<ILogger<CompraBonoController>>();
            ILogger<CompraBonoController> logger = mock.Object;
            var controller = new CompraBonoController(_context, logger);

            // Act
            var actionResult = await controller.CrearCompraBono(dtoConError);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult);

            Assert.NotNull(badRequestResult.Value);

            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);
            Assert.NotNull(problemDetails.Errors);

            var errorActual = problemDetails.Errors.First().Value[0];
            Assert.StartsWith(errorEsperado, errorActual);
        }


        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task CrearCompraBono_TodoBien_Success_test()
        {
            // Arrange
            var mock = new Mock<ILogger<CompraBonoController>>();
            ILogger<CompraBonoController> logger = mock.Object;
            var controller = new CompraBonoController(_context, logger);

            var nombreNuevo = "Usuario";
            var apellido1Nuevo = "Nuevo";
            var apellido2Nuevo = "Test";
            var cantidadPedida = 4;
            var precioBono = _context.BonoBocadillo.Find(BONO_ID_VALIDO).PVP;
            var precioTotalEsperado = precioBono * cantidadPedida;
            var stockEsperado = BONO_STOCK_INICIAL - cantidadPedida;

            var dtoBueno = new ComprarBonoBocadilloPost
            {
                usuario = new UsuarioCompraDTO
                {
                    Nombre = nombreNuevo,
                    Apellido1 = apellido1Nuevo,
                    Apellido2 = apellido2Nuevo
                },
                MetodoPago = MetodoPago.Paypal,
                BonosCompra = new List<BonosCompradosDTO>
                {
                    new BonosCompradosDTO(BONO_ID_VALIDO, BONO_NOMBRE_VALIDO, precioBono, cantidadPedida, "Vegetal")
                }
            };

            // Verificar estado inicial
            Assert.Equal(0, _context.Users.Count());
            Assert.Equal(0, _context.CompraBono.Count());

            // Act
            var actionResult = await controller.CrearCompraBono(dtoBueno);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult);

            Assert.Equal(1, _context.Users.Count());
            var usuarioDb = _context.Users.First();
            Assert.Equal(nombreNuevo, usuarioDb.Nombre);
            Assert.Equal(apellido1Nuevo, usuarioDb.Apellido1);
            Assert.Equal(apellido2Nuevo, usuarioDb.Apellido2);

            Assert.Equal(1, _context.CompraBono.Count());
            var compraDb = _context.CompraBono.First();
            Assert.Equal((float)precioTotalEsperado, compraDb.PrecioTotalBono);
            Assert.Equal(MetodoPago.Paypal, compraDb.metodoPago);

            Assert.Equal(1, _context.Set<BonosComprados>().Count());
            var itemDb = _context.Set<BonosComprados>().First();
            Assert.Equal(compraDb.CompraBonoId, itemDb.CompraId);
            Assert.Equal(BONO_ID_VALIDO, itemDb.BonoId);
            Assert.Equal(cantidadPedida, itemDb.Cantidad);

            var bonoDb = _context.BonoBocadillo.Find(BONO_ID_VALIDO);
            Assert.Equal(stockEsperado, bonoDb.cantidadDisponible);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task CrearCompraBono_UsuarioExistente_Success_test()
        {
            // Arrange
            var mock = new Mock<ILogger<CompraBonoController>>();
            ILogger<CompraBonoController> logger = mock.Object;
            var controller = new CompraBonoController(_context, logger);

            // Crear usuario existente
            var usuarioExistente = new ApplicationUser
            {
                Id = "user-id-1",
                UserName = "juan.perez@test.com",
                Nombre = "Juan",
                Apellido1 = "Pérez",
                Apellido2 = "García"
            };
            _context.Users.Add(usuarioExistente);
            _context.SaveChanges();

            var cantidadPedida = 2;
            var precioBono = _context.BonoBocadillo.Find(BONO_ID_VALIDO).PVP;
            var precioTotalEsperado = precioBono * cantidadPedida;
            var stockEsperado = BONO_STOCK_INICIAL - cantidadPedida;

            var dtoBueno = new ComprarBonoBocadilloPost
            {
                usuario = new UsuarioCompraDTO
                {
                    Nombre = usuarioExistente.Nombre,
                    Apellido1 = usuarioExistente.Apellido1,
                    Apellido2 = usuarioExistente.Apellido2,
                    UserName = usuarioExistente.UserName
                },
                MetodoPago = MetodoPago.Tarjeta,
                BonosCompra = new List<BonosCompradosDTO>
                {
                    new BonosCompradosDTO(BONO_ID_VALIDO, BONO_NOMBRE_VALIDO, precioBono, cantidadPedida, "Vegetal")
                }
            };

            // Verificar estado inicial
            Assert.Equal(1, _context.Users.Count());
            Assert.Equal(0, _context.CompraBono.Count());

            // Act
            var actionResult = await controller.CrearCompraBono(dtoBueno);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult);

            // No se crea nuevo usuario
            Assert.Equal(1, _context.Users.Count());
            var usuarioDb = _context.Users.First();
            Assert.Equal(usuarioExistente.Id, usuarioDb.Id);

            Assert.Equal(1, _context.CompraBono.Count());
            var compraDb = _context.CompraBono.First();
            Assert.Equal((float)precioTotalEsperado, compraDb.PrecioTotalBono);
            Assert.Equal(MetodoPago.Tarjeta, compraDb.metodoPago);

            var bonoDb = _context.BonoBocadillo.Find(BONO_ID_VALIDO);
            Assert.Equal(stockEsperado, bonoDb.cantidadDisponible);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task CrearCompraBono_MultiplesBonosEnUnaCompra_Success_test()
        {
            // Arrange
            var mock = new Mock<ILogger<CompraBonoController>>();
            ILogger<CompraBonoController> logger = mock.Object;
            var controller = new CompraBonoController(_context, logger);

            var cantidadBono1 = 2;
            var cantidadBono2 = 1;
            var precioBono1 = _context.BonoBocadillo.Find(BONO_ID_VALIDO).PVP;
            var precioBono2 = _context.BonoBocadillo.Find(BONO_ID_POCO_STOCK).PVP;
            var precioTotalEsperado = (precioBono1 * cantidadBono1) + (precioBono2 * cantidadBono2);

            var dtoBueno = new ComprarBonoBocadilloPost
            {
                usuario = new UsuarioCompraDTO
                {
                    Nombre = "Usuario",
                    Apellido1 = "Multiple",
                    Apellido2 = "Test"
                },
                MetodoPago = MetodoPago.GooglePay,
                BonosCompra = new List<BonosCompradosDTO>
                {
                    new BonosCompradosDTO(BONO_ID_VALIDO, BONO_NOMBRE_VALIDO, precioBono1, cantidadBono1, "Vegetal"),
                    new BonosCompradosDTO(BONO_ID_POCO_STOCK, BONO_NOMBRE_POCO_STOCK, precioBono2, cantidadBono2, "Mixto")
                }
            };

            // Verificar estado inicial
            Assert.Equal(0, _context.Users.Count());
            Assert.Equal(0, _context.CompraBono.Count());

            // Act
            var actionResult = await controller.CrearCompraBono(dtoBueno);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult);

            Assert.Equal(1, _context.CompraBono.Count());
            var compraDb = _context.CompraBono.First();
            Assert.Equal((float)precioTotalEsperado, compraDb.PrecioTotalBono);
            Assert.Equal(cantidadBono1 + cantidadBono2, compraDb.nBonos);

            Assert.Equal(2, _context.Set<BonosComprados>().Count());

            var bono1Db = _context.BonoBocadillo.Find(BONO_ID_VALIDO);
            Assert.Equal(BONO_STOCK_INICIAL - cantidadBono1, bono1Db.cantidadDisponible);

            var bono2Db = _context.BonoBocadillo.Find(BONO_ID_POCO_STOCK);
            Assert.Equal(BONO_STOCK_INICIAL_POCO - cantidadBono2, bono2Db.cantidadDisponible);
        }
    }
}
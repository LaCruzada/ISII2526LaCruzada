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
    public class ComprarBonoController_test : AppForMovies.UT.AppForMovies4SqliteUT
    {
        private const string _userName = "juan.perez@uclm.es";
        private const string _nombreCliente = "Juan";
        private const string _apellido1Cliente = "Pérez";
        private const string _apellido2Cliente = "García";

        private const string _bono1Nombre = "Bono Vegetal 5";
        private const string _tipo1Nombre = "Vegetal";
        private const string _bono2Nombre = "Bono Mixto 10";
        private const string _tipo2Nombre = "Mixto";
        private const string _bono3Nombre = "Bono Vegetal Premium";

        public ComprarBonoController_test()
        {
            var tiposBocadillo = new List<TipoBocadillo>()
            {
                new TipoBocadillo(1, _tipo1Nombre, new List<BonoBocadillo>()),
                new TipoBocadillo(2, _tipo2Nombre, new List<BonoBocadillo>()),
            };

            var bonos = new List<BonoBocadillo>()
            {
                new BonoBocadillo(1, 10, 5, _bono1Nombre, 25.0f, tiposBocadillo[0], new List<BonosComprados>()),
                new BonoBocadillo(2, 0, 10, _bono2Nombre, 45.0f, tiposBocadillo[1], new List<BonosComprados>()),
                new BonoBocadillo(3, 5, 8, _bono3Nombre, 35.0f, tiposBocadillo[0], new List<BonosComprados>()),
            };

            ApplicationUser user = new ApplicationUser
            {
                Id = "1",
                UserName = _userName,
                Nombre = _nombreCliente,
                Apellido1 = _apellido1Cliente,
                Apellido2 = _apellido2Cliente
            };

            var compraBono = new CompraBono(
                1,
                DateTime.Today,
                2,
                50.0f,
                MetodoPago.Tarjeta,
                new List<BonosComprados>(),
                new List<ApplicationUser> { user }
            );

            _context.TipoBocadillos.AddRange(tiposBocadillo);
            _context.BonoBocadillo.AddRange(bonos);
            _context.Users.Add(user);
            _context.CompraBono.Add(compraBono);
            _context.SaveChanges();
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetBonoParaCompra_SinFiltros_Success_test()
        {
            // Arrange
            var mock = new Mock<ILogger<CompraBonoController>>();
            ILogger<CompraBonoController> logger = mock.Object;

            var controller = new CompraBonoController(_context, logger);

            // Act
            var result = await controller.GetBonoParaCompra(null, null);

            //Assert
            
            var okResult = Assert.IsType<OkObjectResult>(result);
            var bonos = Assert.IsAssignableFrom<IList<ComprarBonosDTO>>(okResult.Value);

       
            Assert.Equal(3, bonos.Count);
            Assert.Contains(bonos, b => b.Nombre == _bono1Nombre);
            Assert.Contains(bonos, b => b.Nombre == _bono2Nombre);
            Assert.Contains(bonos, b => b.Nombre == _bono3Nombre);

        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetBonoParaCompra_FiltroNombre_Success_test()
        {
            // Arrange
            var mock = new Mock<ILogger<CompraBonoController>>();
            ILogger<CompraBonoController> logger = mock.Object;

            var controller = new CompraBonoController(_context, logger);

            // Act
            var result = await controller.GetBonoParaCompra("Vegetal", null);

            //Assert
        
            var okResult = Assert.IsType<OkObjectResult>(result);
            var bonos = Assert.IsAssignableFrom<IList<ComprarBonosDTO>>(okResult.Value);

            Assert.Equal(2, bonos.Count);
            Assert.All(bonos, b => Assert.Contains("Vegetal", b.Nombre));
            Assert.Contains(bonos, b => b.Nombre == _bono1Nombre);
            Assert.Contains(bonos, b => b.Nombre == _bono3Nombre);

        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetBonoParaCompra_FiltroTipo_Success_test()
        {
            // Arrange
            var mock = new Mock<ILogger<CompraBonoController>>();
            ILogger<CompraBonoController> logger = mock.Object;

            var controller = new CompraBonoController(_context, logger);

            // Act
            var result = await controller.GetBonoParaCompra(null, _tipo2Nombre);

            //Assert
        
            var okResult = Assert.IsType<OkObjectResult>(result);
            var bonos = Assert.IsAssignableFrom<IList<ComprarBonosDTO>>(okResult.Value);

           
            Assert.Single(bonos);
            Assert.Equal(_bono2Nombre, bonos[0].Nombre);
            Assert.Equal(_tipo2Nombre, bonos[0].Tipo);

        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetBonoParaCompra_FiltroNombreYTipo_Success_test()
        {
            // Arrange
            var mock = new Mock<ILogger<CompraBonoController>>();
            ILogger<CompraBonoController> logger = mock.Object;

            var controller = new CompraBonoController(_context, logger);

            // Act
            var result = await controller.GetBonoParaCompra("Vegetal", _tipo1Nombre);

            //Assert
      
            var okResult = Assert.IsType<OkObjectResult>(result);
            var bonos = Assert.IsAssignableFrom<IList<ComprarBonosDTO>>(okResult.Value);

         
            Assert.Equal(2, bonos.Count);
            Assert.All(bonos, b =>
            {
                Assert.Contains("Vegetal", b.Nombre);
                Assert.Equal(_tipo1Nombre, b.Tipo);
            });

        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetBonoParaCompra_NoResults_NotFound_test()
        {
            // Arrange
            var mock = new Mock<ILogger<CompraBonoController>>();
            ILogger<CompraBonoController> logger = mock.Object;

            var controller = new CompraBonoController(_context, logger);

            // Act
            var result = await controller.GetBonoParaCompra("Inexistente", null);

            //Assert
         
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No hay bonos que cumplan los requisitos", notFoundResult.Value);

        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetBonoParaCompra_VerificarDatosDTO_Success_test()
        {
            // Arrange
            var mock = new Mock<ILogger<CompraBonoController>>();
            ILogger<CompraBonoController> logger = mock.Object;

            var controller = new CompraBonoController(_context, logger);

            // Act
            var result = await controller.GetBonoParaCompra(_bono1Nombre, null);

            //Assert
          
            var okResult = Assert.IsType<OkObjectResult>(result);
            var bonos = Assert.IsAssignableFrom<IList<ComprarBonosDTO>>(okResult.Value);

            Assert.Single(bonos);
            var bonoDTO = bonos[0];
            Assert.Equal(1, bonoDTO.BonoID);
            Assert.Equal(_bono1Nombre, bonoDTO.Nombre);
            Assert.Equal(25.0, bonoDTO.PrecioCompra);
            Assert.Equal(1, bonoDTO.Cantidad); 
            Assert.Equal(_tipo1Nombre, bonoDTO.Tipo);

        }

        public static IEnumerable<object[]> TestCasesFor_CrearCompraBono_Error()
        {
            var compraSinBonos = new ComprarBonoBocadilloPost
            {
                BonosCompra = new List<BonosCompradosDTO>(),
                usuario = new UsuarioCompraDTO
                {
                    Nombre = _nombreCliente,
                    Apellido1 = _apellido1Cliente,
                    Apellido2 = _apellido2Cliente,
                    UserName = _userName
                },
                MetodoPago = MetodoPago.Tarjeta
            };

            var bonosCompra = new List<BonosCompradosDTO>
            {
                new BonosCompradosDTO(1, _bono1Nombre, 25.0, 2, _tipo1Nombre)
            };

            var compraSinNombre = new ComprarBonoBocadilloPost
            {
                BonosCompra = bonosCompra,
                usuario = new UsuarioCompraDTO
                {
                    Nombre = "",
                    Apellido1 = _apellido1Cliente,
                    Apellido2 = _apellido2Cliente,
                    UserName = _userName
                },
                MetodoPago = MetodoPago.Tarjeta
            };

            var compraSinApellido = new ComprarBonoBocadilloPost
            {
                BonosCompra = bonosCompra,
                usuario = new UsuarioCompraDTO
                {
                    Nombre = _nombreCliente,
                    Apellido1 = "",
                    Apellido2 = _apellido2Cliente,
                    UserName = _userName
                },
                MetodoPago = MetodoPago.Tarjeta
            };

            var compraBonoNoExiste = new ComprarBonoBocadilloPost
            {
                BonosCompra = new List<BonosCompradosDTO>
                {
                    new BonosCompradosDTO(999, "Bono Inexistente", 25.0, 1, _tipo1Nombre)
                },
                usuario = new UsuarioCompraDTO
                {
                    Nombre = _nombreCliente,
                    Apellido1 = _apellido1Cliente,
                    Apellido2 = _apellido2Cliente,
                    UserName = _userName
                },
                MetodoPago = MetodoPago.Tarjeta
            };

            var compraBonoSinStock = new ComprarBonoBocadilloPost
            {
                BonosCompra = new List<BonosCompradosDTO>
                {
                    new BonosCompradosDTO(2, _bono2Nombre, 45.0, 1, _tipo2Nombre)
                },
                usuario = new UsuarioCompraDTO
                {
                    Nombre = _nombreCliente,
                    Apellido1 = _apellido1Cliente,
                    Apellido2 = _apellido2Cliente,
                    UserName = _userName
                },
                MetodoPago = MetodoPago.Tarjeta
            };

            var allTests = new List<object[]>
            {
                new object[] { compraSinBonos, "Error! Debes incluir al menos un bono para comprarlo" },
                new object[] { compraSinNombre, "Error! Debes proporcionar Nombre y Apellido1 del cliente (campos obligatorios)." },
                new object[] { compraSinApellido, "Error! Debes proporcionar Nombre y Apellido1 del cliente (campos obligatorios)." },
                new object[] { compraBonoNoExiste, "Error! El bono con nombre Bono Inexistente y con ID 999 no existe en la base de datos." },
                new object[] { compraBonoSinStock, $"Error! El bono con nombre {_bono2Nombre} solo tiene 0 unidades disponibles, pero has seleccionado 1 unidades para comprar." }
            };

            return allTests;
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        [MemberData(nameof(TestCasesFor_CrearCompraBono_Error))]
        public async Task CrearCompraBono_Error_test(ComprarBonoBocadilloPost compraDTO, string errorExpected)
        {
            // Arrange
            var mock = new Mock<ILogger<CompraBonoController>>();
            ILogger<CompraBonoController> logger = mock.Object;

            var controller = new CompraBonoController(_context, logger);

            // Act
            var result = await controller.CrearCompraBono(compraDTO);

            //Assert
        
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);

            var errorActual = problemDetails.Errors.First().Value[0];

            //we check that the expected error message and actual are the same
            Assert.StartsWith(errorExpected, errorActual);

        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task CrearCompraBono_Success_test()
        {
            // Arrange
            var mock = new Mock<ILogger<CompraBonoController>>();
            ILogger<CompraBonoController> logger = mock.Object;

            var controller = new CompraBonoController(_context, logger);

            var compraDTO = new ComprarBonoBocadilloPost
            {
                BonosCompra = new List<BonosCompradosDTO>
                {
                    new BonosCompradosDTO(1, _bono1Nombre, 25.0, 2, _tipo1Nombre)
                },
                usuario = new UsuarioCompraDTO
                {
                    Nombre = _nombreCliente,
                    Apellido1 = _apellido1Cliente,
                    Apellido2 = _apellido2Cliente,
                    UserName = _userName
                },
                MetodoPago = MetodoPago.Tarjeta
            };

            // Act
            var result = await controller.CrearCompraBono(compraDTO);

            //Assert
      
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var actualCompraDetalle = Assert.IsType<ComprarBonoBocadilloDetalle>(createdResult.Value);

            // Verificamos los datos principales de la compra
            Assert.Equal(_nombreCliente, actualCompraDetalle.NombreCliente);
            Assert.Equal(_apellido1Cliente, actualCompraDetalle.Apellido1);
            Assert.Equal(_apellido2Cliente, actualCompraDetalle.Apellido2);
            Assert.Equal(MetodoPago.Tarjeta, actualCompraDetalle.MetodoPago);
            Assert.Single(actualCompraDetalle.BonosComprados);
            Assert.Equal(1, actualCompraDetalle.BonosComprados[0].BonoID);
            Assert.Equal(2, actualCompraDetalle.BonosComprados[0].Cantidad);
            Assert.Equal(25.0, actualCompraDetalle.BonosComprados[0].PrecioUnitario);

        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task CrearCompraBono_UsuarioNuevo_Success_test()
        {
            // Arrange
            var mock = new Mock<ILogger<CompraBonoController>>();
            ILogger<CompraBonoController> logger = mock.Object;

            var controller = new CompraBonoController(_context, logger);

            var compraDTO = new ComprarBonoBocadilloPost
            {
                BonosCompra = new List<BonosCompradosDTO>
                {
                    new BonosCompradosDTO(1, _bono1Nombre, 25.0, 1, _tipo1Nombre)
                },
                usuario = new UsuarioCompraDTO
                {
                    Nombre = "María",
                    Apellido1 = "López",
                    Apellido2 = "Martínez"
                },
                MetodoPago = MetodoPago.Paypal
            };

            // Act
            var result = await controller.CrearCompraBono(compraDTO);

            //Assert
            
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var actualCompraDetalle = Assert.IsType<ComprarBonoBocadilloDetalle>(createdResult.Value);

            // Verificamos que se creó un nuevo usuario
            Assert.Equal("María", actualCompraDetalle.NombreCliente);
            Assert.Equal("López", actualCompraDetalle.Apellido1);
            Assert.Equal("Martínez", actualCompraDetalle.Apellido2);
            Assert.Equal(MetodoPago.Paypal, actualCompraDetalle.MetodoPago);

        }
    }
}
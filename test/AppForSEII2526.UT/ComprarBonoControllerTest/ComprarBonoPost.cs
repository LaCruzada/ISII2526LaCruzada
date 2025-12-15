using AppForMovies.UT;
using AppForSEII2526.API.ComprarBonoBocadilloDTOs;
using AppForSEII2526.API.Controllers.ControllerComprarBono;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.ComprarBonoPost
{
    public class ComprarBonoPost : AppForMovies4SqliteUT
    {
        private const string _clienteNombre = "Ana";
        private const string _clienteApellido1 = "Garcia";
        private const string _clienteApellido2 = "Lopez";
        private const MetodoPago _metodoPago = MetodoPago.Tarjeta;

        private const string _bono1Nombre = "Bono Completo";
        private const string _tipo1Nombre = "Normal";
        private const string _bono2Nombre = "Bono Pequeño";
        private const string _tipo2Nombre = "Sin gluten";

        

        public ComprarBonoPost()
        {
            var tipos = new List<TipoBocadillo>() {
                new TipoBocadillo { nombreTipo = _tipo1Nombre },
                new TipoBocadillo { nombreTipo = _tipo2Nombre },
            };

            var bonos = new List<BonoBocadillo>(){
                new BonoBocadillo { nombre= _bono1Nombre, PVP = 15.0, nBocadillos = 5, cantidadDisponible = 10, tipoBocadillos = tipos[0] },
                new BonoBocadillo { nombre = _bono2Nombre, PVP = 10.0,nBocadillos = 2, cantidadDisponible = 2, tipoBocadillos = tipos[1] },
            };


            _context.AddRange(tipos);
            _context.AddRange(bonos);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_CreateCompra()
        {
            var compraSinBonos = new ComprarBonoBocadilloPost(0, _clienteNombre, _clienteApellido1, _clienteApellido2,
                DateTime.Today, _metodoPago, new List<BonosCompradosDTO>());

            var bonoItems = new List<BonosCompradosDTO>() { new BonosCompradosDTO(2,_bono2Nombre, 10.0, 2, _tipo2Nombre) };
          

            var compraNombreVacio = new ComprarBonoBocadilloPost(0, "", _clienteApellido1, _clienteApellido2,
                DateTime.Today, _metodoPago, bonoItems);

            var compraApellidosVacios = new ComprarBonoBocadilloPost(0, _clienteNombre, "", "",
                DateTime.Today, _metodoPago, bonoItems);

           
            var allTests = new List<object[]>
            {
                new object[] { compraSinBonos, "Error! Debes seleccionar algún bono" },
                new object[] { compraNombreVacio, "Error! El nombre es obligatorio" },
                new object[] { compraApellidosVacios, "Error! Los apellidos son obligatorios" },
            };

            return allTests;
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        [MemberData(nameof(TestCasesFor_CreateCompra))]
        public async Task CrearCompra_Error_test(ComprarBonoBocadilloPost compraDTO, string errorExpected)
        {
            // Arrange
            var mock = new Mock<ILogger<CompraBonoController>>();
            ILogger<CompraBonoController> logger = mock.Object;

            var controller = new CompraBonoController(_context, logger);

            // Act
            var result = await controller.CrearCompra(compraDTO);

            //Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);

            var errorActual = problemDetails.Errors.First().Value[0];

            Assert.StartsWith(errorExpected, errorActual);
        }

     

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task CrearCompra_Success_test()
        {
            // Arrange
            var mock = new Mock<ILogger<CompraBonoController>>();
            ILogger<CompraBonoController> logger = mock.Object;

            var controller = new CompraBonoController(_context, logger);

            var compraDTO = new ComprarBonoBocadilloPost(1, _clienteNombre, _clienteApellido1, _clienteApellido2,
                DateTime.Today, _metodoPago, new List<BonosCompradosDTO>()
                { new BonosCompradosDTO(2, _bono2Nombre, 10.0, 2, _tipo2Nombre) });

            var expectedCompraDetailDTO = new ComprarBonoBocadilloDetalle(1, 
                new ApplicationUser(_clienteNombre, _clienteApellido1, _clienteApellido2),
                _metodoPago,
                DateTime.Today, 20.0,
                new List<BonosCompradosDTO>()
                { new BonosCompradosDTO(2,_bono2Nombre, 10.0, 2, _tipo2Nombre) });

            // Act
            var result = await controller.CrearCompra(compraDTO);

            //Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var actualCompraDetailDTO = Assert.IsType<ComprarBonoBocadilloDetalle>(createdResult.Value);

            Assert.Equal(expectedCompraDetailDTO, actualCompraDetailDTO);
        }
    }
}
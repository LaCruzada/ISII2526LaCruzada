using AppForSEII2526.API.ComprarBonoBocadilloDTOs;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AppForSEII2526.UT.ComprarBonoControllerTest
{
    public class ComprarBonoSelect : AppForMovies.UT.AppForMovies4SqliteUT
    {
        private const string _tipo1Nombre = "Vegetal";
        private const string _tipo2Nombre = "Mixto";
        private const string _tipo3Nombre = "Ibérico";

        public ComprarBonoSelect()
        {
            var tipoVegetal = new TipoBocadillo(1, _tipo1Nombre, new List<BonoBocadillo>());
            var tipoMixto = new TipoBocadillo(2, _tipo2Nombre, new List<BonoBocadillo>());
            var tipoIberico = new TipoBocadillo(3, _tipo3Nombre, new List<BonoBocadillo>());

            _context.TipoBocadillos.AddRange(tipoVegetal, tipoMixto, tipoIberico);

            _context.BonoBocadillo.AddRange(
                new BonoBocadillo(1, 10, 5, "Bono Vegetal 5", 25.0f, tipoVegetal, new List<BonosComprados>()),
                new BonoBocadillo(2, 15, 10, "Bono Vegetal 10", 45.0f, tipoVegetal, new List<BonosComprados>()),
                new BonoBocadillo(3, 0, 10, "Bono Mixto Sin Stock", 40.0f, tipoMixto, new List<BonosComprados>()),
                new BonoBocadillo(4, 8, 8, "Bono Mixto Premium", 50.0f, tipoMixto, new List<BonosComprados>()),
                new BonoBocadillo(5, 5, 15, "Bono Ibérico Especial", 70.0f, tipoIberico, new List<BonosComprados>())
            );
            _context.SaveChanges();
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        [InlineData(null, null, 5)]              // Sin filtros: todos los bonos (5)
        [InlineData("Vegetal", null, 2)]         // Filtro por nombre "Vegetal": 2 bonos
        [InlineData("Mixto", null, 2)]           // Filtro por nombre "Mixto": 2 bonos
        [InlineData("Ibérico", null, 1)]         // Filtro por nombre "Ibérico": 1 bono
        [InlineData(null, "Vegetal", 2)]         // Filtro por tipo Vegetal: 2 bonos
        [InlineData(null, "Mixto", 2)]           // Filtro por tipo Mixto: 2 bonos
        [InlineData(null, "Ibérico", 1)]         // Filtro por tipo Ibérico: 1 bono
        [InlineData("Vegetal", "Vegetal", 2)]    // Ambos filtros Vegetal: 2 bonos
        [InlineData("Mixto", "Mixto", 2)]        // Ambos filtros Mixto: 2 bonos
        [InlineData("Premium", "Mixto", 1)]      // Nombre "Premium" y tipo Mixto: 1 bono
        [InlineData("Vegetal", "Mixto", 0)]      // Nombre Vegetal pero tipo Mixto: 0 (NotFound)
        public async Task GetBonoParaCompra_FiltrosVariados_DevuelveResultadosCorrectos(string? filtroNombre, string? tipoBocadillo, int expectedCount)
        {
            // Arrange
            var mock = new Mock<ILogger<CompraBonoController>>();
            ILogger<CompraBonoController> logger = mock.Object;
            var controller = new CompraBonoController(_context, logger);

            // Act
            var actionResult = await controller.GetBonoParaCompra(filtroNombre, tipoBocadillo);

            // Assert
            if (expectedCount > 0)
            {
                var okResult = Assert.IsType<OkObjectResult>(actionResult);
                var bonos = Assert.IsAssignableFrom<IList<ComprarBonosDTO>>(okResult.Value);
                Assert.Equal(expectedCount, bonos.Count);

                // Verificar que todos los bonos cumplen el filtro de nombre si aplica
                if (!string.IsNullOrEmpty(filtroNombre))
                {
                    Assert.All(bonos, b => Assert.Contains(filtroNombre, b.Nombre));
                }

                // Verificar que todos los bonos cumplen el filtro de tipo si aplica
                if (!string.IsNullOrEmpty(tipoBocadillo))
                {
                    Assert.All(bonos, b => Assert.Equal(tipoBocadillo, b.Tipo));
                }
            }
            else
            {
                var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult);
                Assert.Equal("No hay bonos que cumplan los requisitos", notFoundResult.Value);
            }
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetBonoParaCompra_SinResultados_DevuelveNotFound()
        {
            // Arrange
            var mock = new Mock<ILogger<CompraBonoController>>();
            ILogger<CompraBonoController> logger = mock.Object;
            var controller = new CompraBonoController(_context, logger);

            // Act
            var actionResult = await controller.GetBonoParaCompra("NoExiste", null);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult);
            Assert.Equal("No hay bonos que cumplan los requisitos", notFoundResult.Value);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetBonoParaCompra_TodosLosBonos_VerificarOrden()
        {
            // Arrange
            var mock = new Mock<ILogger<CompraBonoController>>();
            ILogger<CompraBonoController> logger = mock.Object;
            var controller = new CompraBonoController(_context, logger);

            // Act
            var actionResult = await controller.GetBonoParaCompra(null, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            var bonos = Assert.IsAssignableFrom<IList<ComprarBonosDTO>>(okResult.Value);

           
            var nombresOrdenados = bonos.Select(b => b.Nombre).ToList();
            var nombresEsperados = nombresOrdenados.OrderBy(n => n).ToList();
            Assert.Equal(nombresEsperados, nombresOrdenados);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetBonoParaCompra_VerificarDTOCompleto()
        {
            // Arrange
            var mock = new Mock<ILogger<CompraBonoController>>();
            ILogger<CompraBonoController> logger = mock.Object;
            var controller = new CompraBonoController(_context, logger);

            // Act
            var actionResult = await controller.GetBonoParaCompra("Bono Ibérico Especial", _tipo3Nombre);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            var bonos = Assert.IsAssignableFrom<IList<ComprarBonosDTO>>(okResult.Value);

            Assert.Single(bonos);
            var bono = bonos[0];
            Assert.Equal(5, bono.BonoID);
            Assert.Equal("Bono Ibérico Especial", bono.Nombre);
            Assert.Equal(70.0, bono.PrecioCompra);
            Assert.Equal(1, bono.Cantidad); // Cantidad por defecto
            Assert.Equal(_tipo3Nombre, bono.Tipo);
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        [InlineData("Vegetal", 2)]
        [InlineData("Mixto", 2)]
        [InlineData("Ibérico", 1)]
        public async Task GetBonoParaCompra_FiltroPorTipo_CuentaCorrecta(string tipoBocadillo, int expectedCount)
        {
            // Arrange
            var mock = new Mock<ILogger<CompraBonoController>>();
            ILogger<CompraBonoController> logger = mock.Object;
            var controller = new CompraBonoController(_context, logger);

            // Act
            var actionResult = await controller.GetBonoParaCompra(null, tipoBocadillo);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            var bonos = Assert.IsAssignableFrom<IList<ComprarBonosDTO>>(okResult.Value);
            Assert.Equal(expectedCount, bonos.Count);
            Assert.All(bonos, b => Assert.Equal(tipoBocadillo, b.Tipo));
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetBonoParaCompra_IncluirBonosSinStock_DevuelveTodos()
        {
            // Arrange
            var mock = new Mock<ILogger<CompraBonoController>>();
            ILogger<CompraBonoController> logger = mock.Object;
            var controller = new CompraBonoController(_context, logger);

            // Act
            var actionResult = await controller.GetBonoParaCompra(null, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            var bonos = Assert.IsAssignableFrom<IList<ComprarBonosDTO>>(okResult.Value);

            // Verificar que se incluye el bono sin stock (ID 3)
            Assert.Equal(5, bonos.Count);
            Assert.Contains(bonos, b => b.BonoID == 3 && b.Nombre == "Bono Mixto Sin Stock");
        }
    }
}
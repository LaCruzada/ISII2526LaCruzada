using AppForMovies.UT;
using AppForSEII2526.API.ComprarBonoBocadilloDTOs;
using AppForSEII2526.API.Controllers.ControllerComprarBono;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.ComprarBonoSelect
{
    public class ComprarBonoSelect: AppForMovies4SqliteUT
    {
        public ComprarBonoSelect()
        {
            // Tipos de bocadillo
            var tipos = new List<TipoBocadillo>()
            {
                new TipoBocadillo { idTipo= 1, nombreTipo = "Vegano" },
                new TipoBocadillo { idTipo = 2, nombreTipo = "Normal" },
                new TipoBocadillo { idTipo = 3, nombreTipo = "Sin gluten" }
            };

            // Bonos con IDs específicos para coincidir con las expectativas del test
            var bonos = new List<BonoBocadillo>()
            {
                new BonoBocadillo { BonoId = 1, nombre = "Bono Alpha", PVP = 5.0, nBocadillos = 2, cantidadDisponible = 10, tipoBocadillos = tipos[0] },
                new BonoBocadillo { BonoId = 2, nombre = "Bono Beta", PVP = 12.0, nBocadillos = 3, cantidadDisponible = 5, tipoBocadillos = tipos[1] },
                new BonoBocadillo { BonoId = 3, nombre = "Super Bono", PVP = 15.0, nBocadillos = 5, cantidadDisponible = 2, tipoBocadillos = tipos[2] }
            };

            _context.AddRange(tipos);
            _context.AddRange(bonos);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_GetBonos_OK()
        {
         
            var expectedAll = new List<ComprarBonosDTO>()
            {
                new ComprarBonosDTO(1, "Bono Alpha", 5, 2, "Vegano"),
                new ComprarBonosDTO(2, "Bono Beta", 12.0, 3, "Normal"),
                new ComprarBonosDTO(3, "Super Bono", 15.0, 5, "Sin gluten")
            }.OrderBy(x => x.Nombre).ToList();

          
            var expectedFilterName = expectedAll.Where(b => b.Nombre.Contains("Bono")).OrderBy(x => x.Nombre).ToList();

     
            var expectedTipoNormal = expectedAll.Where(b => b.Tipo == "Normal").ToList();

            return new List<object[]>
            {
                new object[] { null, null, expectedAll },
                new object[] { "Bono", null, expectedFilterName },
                new object[] { null, "Normal", expectedTipoNormal }
            };
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_GetBonos_OK))]
        public async Task GetBonoParaCompra_OK_test(string? filtroNombre, string? tipoBocadillo, IList<ComprarBonosDTO> expected)
        {
            // Arrange
            var mock = new Mock<ILogger<BonoController>>();
            var controller = new BonoController(_context, mock.Object);

            // Act
            var result = await controller.GetBonoParaCompra(filtroNombre, tipoBocadillo);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actual = Assert.IsType<List<ComprarBonosDTO>>(okResult.Value);

           
            var expectedTuples = expected.Select(e => (e.Nombre, e.PrecioCompra, e.Cantidad, e.Tipo)).ToList();
            var actualTuples = actual.Select(a => (a.Nombre, a.PrecioCompra, a.Cantidad, a.Tipo)).ToList();

            Assert.Equal(expectedTuples, actualTuples);
        }

        [Fact]
        public async Task GetBonoParaCompra_NotFound_test()
        {
            // Arrange
            var mock = new Mock<ILogger<BonoController>>();
            var controller = new BonoController(_context, mock.Object);

  
            _context.BonoBocadillo.RemoveRange(_context.BonoBocadillo);
            _context.SaveChanges();

            // Act
            var result = await controller.GetBonoParaCompra(null, null);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);

            // Comprobar que el mensaje exacto devuelto por el controlador está presente
            Assert.Equal("No hay bonos que cumplan los requisitos", notFound.Value);
        }
    }
}
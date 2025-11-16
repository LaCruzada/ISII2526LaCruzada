using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.Controllers.ControllerCrearResenya;
using AppForSEII2526.API.Data;
using AppForSEII2526.API.DTOs;
using AppForSEII2526.API.DTOs.DTOsCrearResenya.BocadillosDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AppForSEII2526.UT.BocadilloController_test
{
    public class BocadilloResenyaControllerTEST : AppForMovies.UT.AppForMovies4SqliteUT
    {
        public BocadilloResenyaControllerTEST()
        {
            _context.TipoPanes.AddRange(
                new TipoPan { PanId = 1, Nombre = "Pan Normal" },
                new TipoPan { PanId = 2, Nombre = "Pan Integral" }
            );
            _context.SaveChanges();


            _context.Bocadillos.AddRange(
                new Bocadillo { Id = 1, Nombre = "Bocata Jamón", PVP = 2.5m, Tamano = EnumTamaño.Normal, TipoPanId = 1 },
                new Bocadillo { Id = 2, Nombre = "Bocata Queso", PVP = 3.0m, Tamano = EnumTamaño.Pequeño, TipoPanId = 2 },
                new Bocadillo { Id = 3, Nombre = "Bocata Mixto", PVP = 4.0m, Tamano = EnumTamaño.Normal, TipoPanId = 1 }
            );
            _context.SaveChanges();
        }


        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [InlineData(null, null, null, 3)]
        [InlineData("Jamón", null, null, 1)]
        [InlineData(null, 3.0, null, 2)]
        [InlineData(null, null, 3.0, 2)]
        [InlineData("Bocata", 2.5, 3.5, 2)]
        [InlineData("NoExiste", null, null, 0)]
        public async Task GetBocadillosParaResenya_Filtrado_DevuelveResultadosCorrectos(string? nombre, double? minPvp, double? maxPvp, int expectedCount)
        {
            var controller = new BocadilloResenyaSelectController(_context);

            // Conversión explícita de double? a decimal?
            decimal? minPvpDecimal = minPvp.HasValue ? (decimal?)minPvp.Value : null;
            decimal? maxPvpDecimal = maxPvp.HasValue ? (decimal?)maxPvp.Value : null;

            var actionResult = await controller.GetBocadillosParaResenya(nombre, minPvpDecimal, maxPvpDecimal);

            if (expectedCount > 0)
            {
                var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
                var bocadillos = Assert.IsType<List<BocadilloSeleccionadoDTO>>(okResult.Value);
                Assert.Equal(expectedCount, bocadillos.Count);
            }
            else
            {
                var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
                Assert.NotNull(notFoundResult.Value);
            }
        }


        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetBocadillosParaResenya_ErrorInterno_DevuelveStatus500()
        {
            var context = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
                .Options);
            var controller = new BocadilloResenyaSelectController(context);

            var actionResult = await controller.GetBocadillosParaResenya(null, null, null);

            var statusResult = Assert.IsType<ObjectResult>(actionResult.Result);
            Assert.Equal(500, statusResult.StatusCode);
        }
    }
}

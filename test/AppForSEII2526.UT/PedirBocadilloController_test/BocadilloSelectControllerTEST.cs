using AppForSEII2526.API.Controllers.ControllerPedirBocadillo; 
using AppForSEII2526.API.Controllers;
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
    public class BocadilloSelectControllerTEST : AppForMovies.UT.AppForMovies4SqliteUT
    {
        public BocadilloSelectControllerTEST()
        {
            var tipoPanBarra = new TipoPan { PanId = 1, Nombre = "Barra" };
            var tipoPanBimbo = new TipoPan { PanId = 2, Nombre = "Bimbo" };

            _context.TipoPanes.AddRange(tipoPanBarra, tipoPanBimbo);

            _context.Bocadillos.AddRange(
                new Bocadillo { Id = 1, Nombre = "Bocata Normal Barra", Tamano = EnumTamaño.Normal, Stock = 10, TipoPan = tipoPanBarra, PVP = 10.0m },
                new Bocadillo { Id = 2, Nombre = "Bocata Pequeño Bimbo", Tamano = EnumTamaño.Pequeño, Stock = 10, TipoPan = tipoPanBimbo, PVP = 15.0m },
                new Bocadillo { Id = 3, Nombre = "Bocata Normal Sin Stock", Tamano = EnumTamaño.Normal, Stock = 0, TipoPan = tipoPanBarra, PVP = 10.0m },
                new Bocadillo { Id = 4, Nombre = "Bocata Normal Bimbo", Tamano = EnumTamaño.Normal, Stock = 10, TipoPan = tipoPanBimbo, PVP = 20.0m }
            );
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> Datos_GetBocadillos_Success()
        {
        
            var dto1 = new BocadilloSelectDTO { Id = 1, Nombre = "Bocata Normal Barra", Tamano = "Normal", Stock = 10, TipoPan = "Barra", Precio = 10.0m };
            var dto2 = new BocadilloSelectDTO { Id = 2, Nombre = "Bocata Pequeño Bimbo", Tamano = "Pequeño", Stock = 10, TipoPan = "Bimbo", Precio = 15.0m };
            var dto4 = new BocadilloSelectDTO { Id = 4, Nombre = "Bocata Normal Bimbo", Tamano = "Normal", Stock = 10, TipoPan = "Bimbo", Precio = 20.0m };

            var listaTodos = new List<BocadilloSelectDTO> { dto1, dto2, dto4 };
            var listaNormales = new List<BocadilloSelectDTO> { dto1, dto4 };
            var listaPequeños = new List<BocadilloSelectDTO> { dto2 };
            var listaBarra = new List<BocadilloSelectDTO> { dto1 };
            var listaBimbo = new List<BocadilloSelectDTO> { dto2, dto4 };
            var listaNormalYBimbo = new List<BocadilloSelectDTO> { dto4 };

            yield return new object[] { null, null, listaTodos };       
            yield return new object[] { "Normal", null, listaNormales }; 
            yield return new object[] { "Pequeño", null, listaPequeños };
            yield return new object[] { null, 1, listaBarra };          
            yield return new object[] { null, 2, listaBimbo };          
            yield return new object[] { "Normal", 2, listaNormalYBimbo };
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [MemberData(nameof(Datos_GetBocadillos_Success))] 
        public async Task GetBocadillosDisponibles_ConResultados_DevuelveOk(
            string? tamano,
            int? tipoPanId,
            List<BocadilloSelectDTO> expectedList)
        {
            var controller = new BocadilloSelectController(_context);

            var actionResult = await controller.GetBocadillosDisponibles(tamano, tipoPanId);

            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var actualList = Assert.IsType<List<BocadilloSelectDTO>>(okResult.Value);

            Assert.Equal(expectedList, actualList);
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [InlineData("Pequeño", 1)] 
       
        public async Task GetBocadillosDisponibles_SinResultados_DevuelveNotFound(string? tamano, int? tipoPanId)
        {
            var controller = new BocadilloSelectController(_context);
            var actionResult = await controller.GetBocadillosDisponibles(tamano, tipoPanId);
            Assert.IsType<NotFoundObjectResult>(actionResult.Result);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetBocadillosDisponibles_TamanoInvalido_DevuelveBadRequest()
        {
            var controller = new BocadilloSelectController(_context);
            var filtroInvalido = "Grande"; 
            var actionResult = await controller.GetBocadillosDisponibles(filtroInvalido, null);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.NotNull(badRequestResult.Value);
        }
    }
}
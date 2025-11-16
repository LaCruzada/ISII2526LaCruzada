using AppForSEII2526.API.Controllers.ControllerCrearResenya;
using AppForSEII2526.API.Data;
using AppForSEII2526.API.DTOs.DTOsCrearResenya.ResenyaDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace AppForSEII2526.UT.ResenyaController_test
{
    public class ResenyaControllerGetDetailsTEST
    {
        private readonly ApplicationDbContext _context;

        public ResenyaControllerGetDetailsTEST()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite("Filename=:memory:")
                .EnableSensitiveDataLogging()
                .Options;

            _context = new ApplicationDbContext(options);
            _context.Database.OpenConnection();
            _context.Database.EnsureCreated();

            var usuario = new ApplicationUser
            {
                Id = "user-test-id",
                UserName = "usuario@ejemplo.com",
                Email = "usuario@ejemplo.com",
                Nombre = "Usuario de Prueba",
                Apellido1 = "ApellidoTest",
                Apellido2 = "Apellido2Test",
                DireccionEnvio = "Calle Falsa 123"
            };
            _context.Users.Add(usuario);
            _context.SaveChanges();

            var tipoPan = new TipoPan { Nombre = "Pan Normal" };
            _context.TipoPanes.Add(tipoPan);
            _context.SaveChanges();

            var bocadillo = new Bocadillo
            {
                Nombre = "Bocata Jamón",
                PVP = 2.5m,
                Tamano = EnumTamaño.Normal,
                TipoPanId = tipoPan.PanId
            };
            _context.Bocadillos.Add(bocadillo);
            _context.SaveChanges();

            var resenya = new Resenya
            {
                Descripcion = "Muy bueno",
                FechaPublicacion = DateTime.Now,
                NombreUsuario = "Miguel",
                Titulo = "Reseña Test",
                Valoracion_General = (EnumValoracion_General)4,
            };
            _context.Resenyas.Add(resenya);
            _context.SaveChanges();

            var resenyaBocadillo = new ResenyaBocadillo
            {
                BocadilloId = bocadillo.Id,
                ResenyaId = resenya.Id,
                Puntuacion = 5
            };
            _context.ResenyaBocadillo.Add(resenyaBocadillo);
            _context.SaveChanges();
        }
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetResenya_Muestra_Success_test()
        {
            var controller = new CrearResenyaController(_context);

            var actionResult = await controller.GetResenya(1);

            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var dto = Assert.IsType<ResenyaDetallesDTO>(okResult.Value);

            Assert.Equal(1, dto.Id);
            Assert.Equal("Reseña Test", dto.Titulo);
            Assert.Single(dto.ResenyaBocadillos);
            Assert.Equal(1, dto.ResenyaBocadillos[0].BocadilloId);
        }

        // TEST 2
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetResenya_NoExiste_DevuelveNotFound()
        {
            var controller = new CrearResenyaController(_context);

            var actionResult = await controller.GetResenya(999);

            var notFound = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.NotNull(notFound.Value);
        }
    }
}

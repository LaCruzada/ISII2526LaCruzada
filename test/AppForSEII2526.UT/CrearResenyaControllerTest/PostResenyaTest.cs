using Xunit;
using Microsoft.EntityFrameworkCore;
using AppForSEII2526.API.Data;
using AppForSEII2526.API.Models;
using AppForSEII2526.API.Controllers.ControllerCrearResenya;
using AppForSEII2526.API.DTOs.DTOsCrearResenya.ResenyaDTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526.UT.CrearResenyaControllerTest
{
    public class PostResenyaTest
    {
        private readonly ApplicationDbContext _context;

        public PostResenyaTest()
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
                Nombre = "Usuario",
                Apellido1 = "Prueba",
                Apellido2 = ""
            };
            _context.Users.Add(usuario);

            var tipoPan = new TipoPan { Nombre = "Pan Normal" };
            _context.TipoPanes.Add(tipoPan);
            _context.SaveChanges();

            var bocadillo = new Bocadillo
            {
                Nombre = "Bocadillo Jamón",
                PVP = 3.5m,
                Tamano = EnumTamaño.Normal,
                TipoPanId = tipoPan.PanId
            };
            _context.Bocadillos.Add(bocadillo);
            _context.SaveChanges();
        }

        [Fact]
        public async Task CrearResenya_OK()
        {
            var controller = new CrearResenyaController(_context);

            var createDto = new ResenyaForCreacionDTO
            {
                Titulo = "Sugerencia para",
                Descripcion = "Muy bueno",
                ValoracionGeneral = EnumValoracion_General.Cuatro,
                NombreUsuario = "usuario@ejemplo.com",
                ResenyaBocadillos = new List<ResenyaBocadilloDTO>
                {
                    new ResenyaBocadilloDTO { BocadilloId = 1, Puntuacion = 5 }
                }
            };

            var result = await controller.CrearResenya(createDto);

            var created = Assert.IsType<CreatedAtActionResult>(result);
            var dto = Assert.IsType<ResenyaDetallesDTO>(created.Value);

            Assert.Equal("Sugerencia para", dto.Titulo);
            Assert.Single(dto.ResenyaBocadillos);
            Assert.Equal(1, dto.ResenyaBocadillos[0].BocadilloId);
        }

        [Fact]
        public async Task CrearResenya_SinDescripción_Error()
        {
            var controller = new CrearResenyaController(_context);

            var dto = new ResenyaForCreacionDTO
            {
                Titulo = "Sugerencia para",
                Descripcion = "",
                ValoracionGeneral = EnumValoracion_General.Una,
                ResenyaBocadillos = new List<ResenyaBocadilloDTO>
                {
                    new ResenyaBocadilloDTO { BocadilloId = 1, Puntuacion = 4 }
                }
            };

            var result = await controller.CrearResenya(dto);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var problem = Assert.IsType<ValidationProblemDetails>(badRequest.Value);

            Assert.True(problem.Errors.ContainsKey("DescripciónObligatoria"));
        }

        [Fact]
        public async Task CrearResenya_SinBocadillos_Error()
        {
            var controller = new CrearResenyaController(_context);

            var dto = new ResenyaForCreacionDTO
            {
                Titulo = "Sugerencia para",
                Descripcion = "Desc",
                ValoracionGeneral = EnumValoracion_General.Tres,
                ResenyaBocadillos = null
            };

            var result = await controller.CrearResenya(dto);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var problem = Assert.IsType<ValidationProblemDetails>(badRequest.Value);

            Assert.True(problem.Errors.ContainsKey("ResenyaBocadillos"));
        }

        [Fact]
        public async Task CrearResenya_UsuarioNuevo_SeCrea()
        {
            var controller = new CrearResenyaController(_context);

            var dto = new ResenyaForCreacionDTO
            {
                Titulo = "Sugerencia para",
                Descripcion = "Desc",
                ValoracionGeneral = EnumValoracion_General.Cinco,
                NombreUsuario = "nuevo@correo.com",
                ResenyaBocadillos = new List<ResenyaBocadilloDTO>
                {
                    new ResenyaBocadilloDTO { BocadilloId = 1, Puntuacion = 5 }
                }
            };

            var result = await controller.CrearResenya(dto);

            Assert.IsType<CreatedAtActionResult>(result);
            var usuarioDb = _context.Users.FirstOrDefault(u => u.UserName == "nuevo@correo.com");
            Assert.NotNull(usuarioDb);
            Assert.Equal("", usuarioDb.Apellido2);
        }

        [Fact]
        public async Task CrearResenya_BocadilloInexistente_Error()
        {
            var controller = new CrearResenyaController(_context);

            var dto = new ResenyaForCreacionDTO
            {
                Titulo = "Sugerencia para",
                Descripcion = "Desc",
                ValoracionGeneral = EnumValoracion_General.Una,
                ResenyaBocadillos = new List<ResenyaBocadilloDTO>
                {
                    new ResenyaBocadilloDTO { BocadilloId = 999, Puntuacion = 5 }
                }
            };

            var result = await controller.CrearResenya(dto);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var problem = Assert.IsType<ValidationProblemDetails>(badRequest.Value);

            Assert.Contains("Bocadillo 999 no encontrado.", problem.Errors["ResenyaBocadillos"].First());
        }

        [Fact]
        public async Task CrearResenya_TituloIncorrecto_Error()
        {
            var controller = new CrearResenyaController(_context);

            var dto = new ResenyaForCreacionDTO
            {
                Titulo = "Modificar el bocadillo",
                Descripcion = "Mal titulo",
                ValoracionGeneral = EnumValoracion_General.Una,
                ResenyaBocadillos = new List<ResenyaBocadilloDTO>
                {
                    new ResenyaBocadilloDTO { BocadilloId = 1, Puntuacion = 4 }
                }
            };

            var result = await controller.CrearResenya(dto);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var problem = Assert.IsType<ValidationProblemDetails>(badRequest.Value);

            Assert.True(problem.Errors.ContainsKey("TituloObligatorio"));
        }

        [Fact]
        public async Task CrearResenya_SinTitulo_Error()
        {
            var controller = new CrearResenyaController(_context);

            var dto = new ResenyaForCreacionDTO
            {
                Titulo = "",
                Descripcion = "Sin titulo",
                ValoracionGeneral = EnumValoracion_General.Una,
                ResenyaBocadillos = new List<ResenyaBocadilloDTO>
                {
                    new ResenyaBocadilloDTO { BocadilloId = 1, Puntuacion = 4 }
                }
            };

            var result = await controller.CrearResenya(dto);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var problem = Assert.IsType<ValidationProblemDetails>(badRequest.Value);

            Assert.True(problem.Errors.ContainsKey("TituloObligatorio"));
        }
    }
}

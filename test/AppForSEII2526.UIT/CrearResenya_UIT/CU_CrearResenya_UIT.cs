using AppForMovies.UIT.Shared;
using OpenQA.Selenium;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;
using AppForSEII2526.UIT.Resenya_UIT;
using System.Threading;

namespace AppForSEII2526.UIT.CrearResenya_UIT
{
    public class CU_CrearResenya_UIT : UC_UIT
    {
        private const string bocadillo1Nombre = "Bocadillo Vegano";
        private const string bocadillo2Nombre = "Bocadillo Queso";
        private const string bocadillo3Nombre = "Bocadillo Sin Gluten";

        private const string usuarioNombre = "Ana";
        private const string tituloResenya = "Sugerencia para";
        private const string descripcionResenya = "Muy sabroso y recomendable.";
        private const string valoracionGeneral = "3";

        public CU_CrearResenya_UIT(ITestOutputHelper output) : base(output) { }

        private void Ir_A_CrearResenya()
        {
            _driver.Navigate().GoToUrl(_URI + "Resenya/Select");
            
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        [Trait("CasoPrueba", "ESC-01")]
        public void ESC_01_Crear_Resenya_Estandar()
        {
            var selectPO = new SelectBocadillosResenya_PO(_driver, _output);
            var postPO = new PostCrearResenya_PO(_driver, _output);
            var detallesPO = new DetailsResenya_PO(_driver, _output);

            Ir_A_CrearResenya();

            // Selección de bocadillo
            selectPO.AddBocadilloByName(bocadillo1Nombre);

            // Click en Crear Resenya
            selectPO.WaitForBeingClickable(By.Id("CrearResenya"));
            _driver.FindElement(By.Id("CrearResenya")).Click();

            // Rellenar formulario
            postPO.SetNombreUsuario(usuarioNombre);
            postPO.SetTituloResenya(tituloResenya);
            postPO.SetDescripcionResenya(descripcionResenya);
            postPO.SetValoracionGeneral(valoracionGeneral);
            postPO.SetPuntuacionBocadillo("9");

            // Publicar y confirmar modal
            postPO.PublicarResenya();
            postPO.ConfirmarResenyaaModal();

            // --- Verificación en tabla principal ---
            var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            var tablaResenya = wait.Until(d => d.FindElement(By.CssSelector(".container-fluid > table:first-of-type")));
            var filas = tablaResenya.FindElements(By.TagName("tr"));
            var datos = new Dictionary<string, string>();
            foreach (var fila in filas)
            {
                var th = fila.FindElement(By.TagName("th")).Text.Trim();
                var td = fila.FindElement(By.TagName("td")).Text.Trim();
                datos[th] = td;
            }

            Assert.Equal(usuarioNombre, datos["Usuario"]);
            Assert.Equal(tituloResenya, datos["Título"]);
            Assert.Equal(descripcionResenya, datos["Descripción"]);
            Assert.Equal("Cuatro", datos["Valoración general"]);

            // --- Verificación de bocadillos ---
            var tablaBocadillos = _driver.FindElement(By.CssSelector(".container-fluid > table:nth-of-type(2) tbody"));
            var filasBocadillos = tablaBocadillos.FindElements(By.TagName("tr"));
            bool bocadilloEncontrado = filasBocadillos.Any(fila =>
            {
                var celdas = fila.FindElements(By.TagName("td"));
                if (celdas.Count < 4) return false; // Por si hay fila "No hay bocadillos"
                return celdas[0].Text.Trim() == bocadillo1Nombre &&
                       celdas[3].Text.Trim() == "9"; // Puntuación
            });

            Assert.True(bocadilloEncontrado, "No se encontró el bocadillo evaluado en la tabla.");
        }


        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        [Trait("CasoPrueba", "ESC-02")]
        public void ESC_02_Filtrar_Bocadillos()
        {
            var selectPO = new SelectBocadillosResenya_PO(_driver, _output);

            Ir_A_CrearResenya();
            selectPO.SearchBocadillos("Vegano", 3.50, 3.90);

            Assert.True(selectPO.CheckListOfBocadillos(new List<string> { bocadillo1Nombre }));
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        [Trait("CasoPrueba", "ESC-03")]
        public void ESC_03_Carrito_Vacio_Boton_Deshabilitado()
        {
            var selectPO = new SelectBocadillosResenya_PO(_driver, _output);

            Ir_A_CrearResenya();
            Assert.True(selectPO.CrearResenyaNotAvailable());
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        [Trait("CasoPrueba", "ESC-04")]
        public void ESC_04_Eliminar_Bocadillo_Carrito()
        {
            var selectPO = new SelectBocadillosResenya_PO(_driver, _output);

            Ir_A_CrearResenya();

            selectPO.AddBocadilloByName(bocadillo1Nombre);
            selectPO.AddBocadilloByName(bocadillo2Nombre);
            selectPO.RemoveBocadilloFromCart(bocadillo1Nombre);

            Assert.True(selectPO.IsBocadilloInCart(bocadillo2Nombre));
            Assert.False(selectPO.IsBocadilloInCart(bocadillo1Nombre));
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        [Trait("CasoPrueba", "ESC-05")]
        public void ESC_05_Datos_Obligatorios()
        {
            var selectPO = new SelectBocadillosResenya_PO(_driver, _output);
            var postPO = new PostCrearResenya_PO(_driver, _output);

            Ir_A_CrearResenya();

            selectPO.AddBocadilloByName(bocadillo1Nombre);
            selectPO.WaitForBeingClickable(By.Id("CrearResenya"));
            _driver.FindElement(By.Id("CrearResenya")).Click();
            Thread.Sleep(1000);

            postPO.SetDescripcionResenya(descripcionResenya);
            postPO.PublicarResenya();

            Assert.True(postPO.CheckMessageError("obligatorio"));
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        [Trait("CasoPrueba", "ESC-06")]
        public void ESC_06_Modificar_Bocadillos_Mantiene_Datos()
        {
            var selectPO = new SelectBocadillosResenya_PO(_driver, _output);
            var postPO = new PostCrearResenya_PO(_driver, _output);

            Ir_A_CrearResenya();

            selectPO.AddBocadilloByName(bocadillo1Nombre);
            selectPO.WaitForBeingClickable(By.Id("CrearResenya"));
            _driver.FindElement(By.Id("CrearResenya")).Click();
            Thread.Sleep(1000);

            postPO.SetNombreUsuario(usuarioNombre);
            postPO.SetTituloResenya(tituloResenya);

            postPO.ModificarBocadillos();
            Thread.Sleep(1000);

            Assert.True(_driver.Url.Contains("/Resenya/Select"));
            Assert.True(selectPO.IsBocadilloInCart(bocadillo1Nombre));

            _driver.FindElement(By.Id("CrearResenya")).Click();
            Thread.Sleep(1000);

            var tituloInput = _driver.FindElement(By.Id("TituloResenya"));
            Assert.Equal(tituloResenya, tituloInput.GetAttribute("value"));
        }
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        [Trait("CasoPrueba", "ESC-01")]
        public void ESC_07_Examen_Sprint3()
        {
            var selectPO = new SelectBocadillosResenya_PO(_driver, _output);
            var postPO = new PostCrearResenya_PO(_driver, _output);
            var detallesPO = new DetailsResenya_PO(_driver, _output);

            Ir_A_CrearResenya();

            // Selección de bocadillo Queso
            selectPO.AddBocadilloByName(bocadillo1Nombre);

            // Filtrar por PVP
            selectPO.SearchBocadillos("", 3, 0);

            // Selección de bocadillo Vegano
            selectPO.AddBocadilloByName(bocadillo3Nombre);

            // Eliminar Bocadillo Queso del carrito
            selectPO.RemoveBocadilloFromCart(bocadillo1Nombre);

            // Click en Crear Resenya
            selectPO.WaitForBeingClickable(By.Id("CrearResenya"));
            _driver.FindElement(By.Id("CrearResenya")).Click();

            // Rellenar formulario
            postPO.SetNombreUsuario(usuarioNombre);
            postPO.SetTituloResenya(tituloResenya);
            postPO.SetDescripcionResenya(descripcionResenya);
            postPO.SetValoracionGeneral(valoracionGeneral);
            postPO.SetPuntuacionBocadillo("9");

            // Publicar y confirmar modal
            postPO.PublicarResenya();
            postPO.ConfirmarResenyaaModal();

            // --- Verificación en tabla principal ---
            var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            var tablaResenya = wait.Until(d => d.FindElement(By.CssSelector(".container-fluid > table:first-of-type")));
            var filas = tablaResenya.FindElements(By.TagName("tr"));
            var datos = new Dictionary<string, string>();
            foreach (var fila in filas)
            {
                var th = fila.FindElement(By.TagName("th")).Text.Trim();
                var td = fila.FindElement(By.TagName("td")).Text.Trim();
                datos[th] = td;
            }

            Assert.Equal(usuarioNombre, datos["Usuario"]);
            Assert.Equal(tituloResenya, datos["Título"]);
            Assert.Equal(descripcionResenya, datos["Descripción"]);
            Assert.Equal("Cuatro", datos["Valoración general"]);

            // --- Verificación de bocadillos ---
            var tablaBocadillos = _driver.FindElement(By.CssSelector(".container-fluid > table:nth-of-type(2) tbody"));
            var filasBocadillos = tablaBocadillos.FindElements(By.TagName("tr"));
            bool bocadilloEncontrado = filasBocadillos.Any(fila =>
            {
                var celdas = fila.FindElements(By.TagName("td"));
                return celdas[0].Text.Trim() == bocadillo3Nombre;
            });

            Assert.True(bocadilloEncontrado, "No se encontró el bocadillo evaluado en la tabla.");
        }

    }
}

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
        private const string bocadillo2Nombre = "Bocadillo Sin Gluten";

        private const string usuarioNombre = "Ana";
        private const string tituloResenya = "¡Excelente!";
        private const string descripcionResenya = "Muy sabroso y recomendable.";
        private const string valoracionGeneral = "5";

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

            selectPO.AddBocadilloByName(bocadillo1Nombre);
            selectPO.ClickCrearResenya();

            postPO.SetNombreUsuario(usuarioNombre);
            postPO.SetTituloResenya(tituloResenya);
            postPO.SetDescripcionResenya(descripcionResenya);
            postPO.SetValoracionGeneral(valoracionGeneral);
            postPO.SetPuntuacionBocadillo("1", "9"); // Ejemplo ID bocadillo

            postPO.PublicarResenya();
            detallesPO.WaitForBeingVisible(By.Id("ResumenResenya"));

            Assert.Contains(usuarioNombre, _driver.PageSource);
            Assert.Contains(tituloResenya, _driver.PageSource);
            Assert.Contains(descripcionResenya, _driver.PageSource);
            Assert.Contains(bocadillo1Nombre, _driver.PageSource);
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        [Trait("CasoPrueba", "ESC-02")]
        public void ESC_02_Filtrar_Bocadillos()
        {
            var selectPO = new SelectBocadillosResenya_PO(_driver, _output);

            Ir_A_CrearResenya();
            selectPO.SearchBocadillos("Vegano", null, null);

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
    }
}

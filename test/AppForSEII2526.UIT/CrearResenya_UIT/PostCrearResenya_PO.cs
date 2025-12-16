using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using AppForSEII2526.UIT.Shared;
using Xunit.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace AppForSEII2526.UIT.Resenya_UIT
{
    public class PostCrearResenya_PO : PageObject
    {
        private By _nombreUsuarioBy = By.Id("NombreUsuario");
        private By _tituloResenyaBy = By.Id("TituloResenya");
        private By _descripcionResenyaBy = By.Id("DescripcionResenya");
        private By _valoracionGeneralBy = By.Id("ValoracionGeneral");
        private string _puntuacionBocadilloPrefix = "Puntuacion_";
        private By _botonPublicarBy = By.Id("PublicarResenya");
        private By _botonModificarBocadillosBy = By.Id("ModificarBocadillos");

        private By _tablaResumenBocadillosBy = By.Id("TableResumenBocadillos");

        public PostCrearResenya_PO(IWebDriver driver, ITestOutputHelper output)
            : base(driver, output)
        {
        }

        // ---------------- Helper: Espera a que un elemento esté visible ----------------
        private IWebElement WaitAndFind(By by, int timeoutSeconds = 10)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutSeconds));
            return wait.Until(d => d.FindElement(by));
        }

        // ---------------- Inputs ----------------
        private IWebElement NombreUsuarioInput() => WaitAndFind(_nombreUsuarioBy);
        private IWebElement TituloInput() => WaitAndFind(_tituloResenyaBy);
        private IWebElement DescripcionInput() => WaitAndFind(_descripcionResenyaBy);
        private IWebElement ValoracionGeneralSelect() => WaitAndFind(_valoracionGeneralBy);

        public void SetNombreUsuario(string nombre)
        {
            var input = NombreUsuarioInput();
            input.Clear();
            input.SendKeys(nombre);
        }

        public void SetTituloResenya(string titulo)
        {
            var input = TituloInput();
            input.Clear();
            input.SendKeys(titulo);
        }

        public void SetDescripcionResenya(string descripcion)
        {
            var input = DescripcionInput();
            input.Clear();
            input.SendKeys(descripcion);
        }

        public void SetValoracionGeneral(string estrellas)
        {
            var selectElement = new SelectElement(ValoracionGeneralSelect());
            selectElement.SelectByValue(estrellas);
        }

        public void SetPuntuacionBocadillo(string puntuacion)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            var input = wait.Until(d => d.FindElement(By.CssSelector($"input[id^='{_puntuacionBocadilloPrefix}']")));
            input.Clear();
            input.SendKeys(puntuacion);
        }

        // ---------------- Botones ----------------
        private IWebElement BotonPublicar() => WaitAndFind(_botonPublicarBy);
        private IWebElement BotonModificarBocadillos() => WaitAndFind(_botonModificarBocadillosBy);

        public void PublicarResenya()
        {
            BotonPublicar().Click();
            Thread.Sleep(300); // breve espera para que aparezca modal
        }

        public void ModificarBocadillos()
        {
            BotonModificarBocadillos().Click();
            Thread.Sleep(300);
        }

        public bool IsEnabledPublicar()
        {
            return BotonPublicar().Enabled;
        }

        // ---------------- Modal de confirmación ----------------
        public void ConfirmarResenyaaModal()
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            // Esperar hasta que el modal esté presente
            var modalButton = wait.Until(d =>
            {
                var buttons = d.FindElements(By.CssSelector(".modal-footer .btn-primary"));
                return buttons.Count > 0 ? buttons[0] : null;
            });

            // Click seguro vía JS
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", modalButton);

            // Esperar a que el modal desaparezca
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector(".dialog-container")));
        }

        // ---------------- Comprobaciones ----------------
        public bool IsResumenResenyaVisible()
        {
            try
            {
                WaitAndFind(_tablaResumenBocadillosBy);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool CheckResumenBocadillos(List<string[]> expectedBocadillos)
        {
            var table = WaitAndFind(_tablaResumenBocadillosBy);
            var rows = table.FindElements(By.TagName("tr")).Skip(1).ToList(); // Saltar encabezado

            for (int i = 0; i < expectedBocadillos.Count; i++)
            {
                var expected = expectedBocadillos[i];
                if (i >= rows.Count)
                    return false;

                var cells = rows[i].FindElements(By.TagName("td")).Select(td => td.Text.Trim()).ToArray();
                for (int j = 0; j < expected.Length; j++)
                {
                    if (cells.Length <= j || !cells[j].Equals(expected[j], StringComparison.OrdinalIgnoreCase))
                        return false;
                }
            }
            return true;
        }
        // Nuevo método compatible con Blazor
        public bool CheckMessageError(string errorMessage)
        {
            try
            {
                // Buscar todos los divs de errores
                var erroresDivs = _driver.FindElements(By.CssSelector("div[data-test-id^='ErrorsShown']"));

                foreach (var div in erroresDivs)
                {
                    if (div.Text.Contains(errorMessage, StringComparison.OrdinalIgnoreCase))
                    {
                        _output.WriteLine($"Mensaje mostrado: {div.Text}");
                        return true;
                    }
                }

                _output.WriteLine($"No se encontró el mensaje: {errorMessage}");
                return false;
            }
            catch (NoSuchElementException)
            {
                _output.WriteLine("No se encontraron mensajes de error en la página.");
                return false;
            }
        }

    }
}
